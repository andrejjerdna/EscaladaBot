using EscaladaBot.Contracts;
using EscaladaBot.Services.Contexts;
using EscaladaBot.Services.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EscaladaBot.Services;

public class TelegramBotHandler : ITelegramBotHandler
{
    private readonly IRulesProvider _rulesProvider;
    private readonly IContextSwitcher _contextSwitcher;
    private readonly ILogger<TelegramBotHandler> _logger;
    
    private IContext _context;
    
    public TelegramBotHandler(IRulesProvider rulesProvider,
        IContextSwitcher contextSwitcher,
        ILogger<TelegramBotHandler> logger)
    {
        _rulesProvider = rulesProvider;
        _logger = logger;
        _contextSwitcher = contextSwitcher;
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception?.Message != null)
            _logger.LogWarning(exception.Message);
        
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, 
        Update update,
        CancellationToken cancellationToken)
    {
        if(update.Message?.Chat.Id == null)
            return;

        var chatId = update.Message.Chat.Id;

        _context = await _contextSwitcher.GetContext(new ContextData(chatId, 
            update.Message.Text,
            update.Message?.EntityValues?.FirstOrDefault(),
            update.Message?.Type,
            update.CallbackQuery?.Message));
        
        try
        {
            // var canCreateTrace = await _rulesProvider.CanCreateTrace(chatId, update.Message.Text);
            //
            // if(!canCreateTrace)
            //     return;
           
            var currentCommands = await _context.GetCurrentCommands(chatId);
            
            if(!currentCommands.Any())
                return;

            foreach (var command in currentCommands)
            {
                try
                {
                    var result = await command.Run(botClient, update);

                    if (!result)
                    {
                        await command.NotCompleted(botClient, update);
                    }
                }
                catch (Exception e)
                {
                    await command.Error(botClient, update);
                    throw new Exception($"Error handling command {command.Name}. Error message{e.Message}");
                }
            }
        }
        
        catch(Exception ex)
        {
            if (ex?.Message != null)
                    _logger.LogWarning(ex.Message);
        }
    }
}