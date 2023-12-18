using EscaladaBot.Contracts;
using Telegram.Bot;

namespace EscaladaApi.Persistence;

public sealed class TelegramConnectionFactory : ITelegramConnectionFactory
{
    private readonly ITelegramBotClient _bot;

    public TelegramConnectionFactory(string apiKey)
    {
        _bot = new TelegramBotClient(apiKey);
    }

    public ITelegramBotClient GetClient() => _bot;
}