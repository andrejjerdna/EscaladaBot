using Telegram.Bot;

namespace EscaladaApi.Contracts;

public interface ITelegramConnectionFactory
{
    ITelegramBotClient GetClient();
}