using EscaladaBot.Contracts;
using EscaladaBot.Services.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.Handlers;

public sealed class TelegramBotHandler : ITelegramBotHandler
{
    private readonly IContextSwitcher _contextSwitcher;
    private readonly ILogger<TelegramBotHandler> _logger;
    
    private IContext _context;
    
    public TelegramBotHandler(IContextSwitcher contextSwitcher,
        ILogger<TelegramBotHandler> logger)
    {
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