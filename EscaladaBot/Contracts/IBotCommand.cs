using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaApi.Contracts;

public interface IBotCommand
{
    Task Start(ITelegramBotClient botClient, Update update);
    Task<bool> Create(ITelegramBotClient botClient, Update update);
    Task Error(ITelegramBotClient botClient, Update update);
}