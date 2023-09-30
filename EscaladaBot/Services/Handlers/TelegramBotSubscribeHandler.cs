using EscaladaBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services;

public class TelegramBotSubscribeHandler : ITelegramBotHandler
{
    private readonly ITelegramBotHandler _nextHandler;

    public TelegramBotSubscribeHandler(ITelegramBotHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.Chat.Id == null)
        {
            return _nextHandler.HandleUpdateAsync(botClient, update, cancellationToken);
        }
        
        if (update.Message?.Text is not "subscribe" and not "unsubscribe")
        {
            return _nextHandler.HandleUpdateAsync(botClient, update, cancellationToken);
        }
        
        
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}