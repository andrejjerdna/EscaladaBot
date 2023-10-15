using EscaladaBot.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using UpdateType = Telegram.Bot.Types.Enums.UpdateType;

namespace EscaladaBot.HostedServices;

public sealed class TelegramHostedService : IHostedService
{
    private readonly ITelegramBotClient _bot;
    private readonly ITelegramBotHandler _telegramBotHandler;
    private readonly ILogger<TelegramHostedService> _logger;

    public TelegramHostedService(ITelegramConnectionFactory connectionFactory,
        ILogger<TelegramHostedService> logger,
        ITelegramBotHandler telegramBotHandler)
    {
        _logger = logger;
        _telegramBotHandler = telegramBotHandler;
        _bot = connectionFactory.GetClient();
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Telegram bot ready to start");
        
        _bot.StartReceiving(
            _telegramBotHandler.HandleUpdateAsync,
            _telegramBotHandler.HandleErrorAsync,
            new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] { UpdateType.Message, UpdateType.CallbackQuery },
                ThrowPendingUpdates = true
            },
            CancellationToken.None
        );
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot?.CloseAsync(cancellationToken: cancellationToken)!;
        
        _logger.LogInformation("Telegram bot is closed");
    }
}