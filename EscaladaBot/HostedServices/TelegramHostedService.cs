using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using UpdateType = Telegram.Bot.Types.Enums.UpdateType;

namespace EscaladaApi.Services;

public sealed class TelegramHostedService : IHostedService
{
    private readonly ITelegramBotClient _bot;
    private readonly ILogger<TelegramHostedService> _logger;
    private readonly ICreationManager _creationManager;
    private readonly ITraceInfoViewer _traceInfoViewer;
    private readonly IRulesProvider _rulesProvider;

    public TelegramHostedService(ITelegramConnectionFactory connectionFactory,
        ILogger<TelegramHostedService> logger, 
        ICreationManager creationManager, 
        ITraceInfoViewer traceInfoViewer, 
        IRulesProvider rulesProvider)
    {
        _logger = logger;
        _creationManager = creationManager;
        _traceInfoViewer = traceInfoViewer;
        _rulesProvider = rulesProvider;
        _bot = connectionFactory.GetClient();
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] { UpdateType.Message },
                ThrowPendingUpdates = true
            },
            CancellationToken.None
        );
        
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot?.CloseAsync(cancellationToken: cancellationToken)!;
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception?.Message != null)
                _logger.LogWarning(exception.Message);
        
        return Task.CompletedTask;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, 
        Update update,
        CancellationToken cancellationToken)
    {
        if(update.Message?.Chat.Id == null)
            return;

        var chatId = update.Message.Chat.Id;
        
        try
        {
            var viewResult = await _traceInfoViewer.ViewInfo(chatId, update.Message.Text);
            
            if(viewResult)
                return;

            var canCreateTrace = await _rulesProvider.CanCreateTrace(chatId, update.Message.Text);
            
            if(!canCreateTrace)
                return;
            
            var currentTraceCreator = await _creationManager.GetCurrentTraceCreator(chatId);
            
            if(currentTraceCreator == null)
                return;
            
            var result = await currentTraceCreator.Create(botClient, update);

            if (!result)
            {
                await _creationManager.Remove(chatId);
                await currentTraceCreator.Error(botClient, update);
                return;
            }

            await _creationManager.ToNextStage(chatId);
            
            var nextTraceCreator = await _creationManager.GetCurrentTraceCreator(chatId);

            if (nextTraceCreator == null)
            {
                await _creationManager.Remove(chatId);
                return;
            }

            await nextTraceCreator.Start(botClient, update);
        }
        
        catch(Exception ex)
        {
            await _creationManager.Remove(chatId);
            
            if (ex?.Message != null)
                _logger.LogWarning(ex.Message);
        }
    }
}