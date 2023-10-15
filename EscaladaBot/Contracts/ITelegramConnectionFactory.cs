using Telegram.Bot;

namespace EscaladaBot.Contracts;

public interface ITelegramConnectionFactory
{
    ITelegramBotClient GetClient();
}