using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaApi.Contracts;

public interface IBotCommand
{
    string Name { get; }
    Task<bool> Run(ITelegramBotClient botClient, Update update);
    Task NotCompleted(ITelegramBotClient botClient, Update update);
    Task Error(ITelegramBotClient botClient, Update update);
}