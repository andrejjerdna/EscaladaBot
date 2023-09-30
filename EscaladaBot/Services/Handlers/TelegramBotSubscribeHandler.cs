using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.Handlers;

public sealed class TelegramBotSubscribeHandler : ITelegramBotHandler
{
    private readonly ITelegramBotHandler _nextHandler;
    private readonly ISubscribeRepository _repository;

    private const string SubscribeMessage = "Если появится новая трасса, то я пришлю ее тебе!";
    private const string UnsubscribeMessage = "Жаль, теперь ты не увидишь новых трасс...";

    public TelegramBotSubscribeHandler(ITelegramBotHandler nextHandler, ISubscribeRepository repository)
    {
        _nextHandler = nextHandler;
        _repository = repository;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        switch (update.Message?.Text)
        {
            case "/subscribe":
                await _repository.Subscribe(update.Message.Chat.Id, update.Message.From?.Username ?? "Unknown");
                await botClient.SendMessage(update.Message.Chat.Id, SubscribeMessage);
                return;
            case "/unsubscribe":
                await _repository.Unsubscribe(update.Message.Chat.Id);
                await botClient.SendMessage(update.Message.Chat.Id, UnsubscribeMessage);
                return;
            default:
                await _nextHandler.HandleUpdateAsync(botClient, update, cancellationToken);
                return;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return _nextHandler.HandleErrorAsync(botClient, exception, cancellationToken);
    }
}