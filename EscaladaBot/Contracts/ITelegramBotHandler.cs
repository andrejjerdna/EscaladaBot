using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Contracts;

public interface ITelegramBotHandler
{
    Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken);

    Task HandleErrorAsync(ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken);
}