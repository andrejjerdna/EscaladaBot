using EscaladaBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.Handlers;

public sealed class TelegramBotCallBackHandler : ITelegramBotHandler
{
    private readonly ITelegramBotHandler _nextHandler;
    private readonly IRegisterRepository _registerRepository;

    public TelegramBotCallBackHandler(ITelegramBotHandler nextHandler, IRegisterRepository registerRepository)
    {
        _nextHandler = nextHandler;
        _registerRepository = registerRepository;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var chatId = update.CallbackQuery?.Message?.Chat.Id;

        if (chatId == null)
        {
            await _nextHandler.HandleUpdateAsync(botClient, update, cancellationToken);
            return;
        }

        var data = update.CallbackQuery?.Data;

        if (data == null)
            return;

        var guid = new Guid(data);

        var model = _registerRepository.GetRegisterModel(guid);

        if (model == null)
            return;
        
        await botClient.SendTextMessageAsync(
            chatId: model.ChatId,
            text: $"{model.User} (@{model.Login}) хочет прийти {model.DateTime.ToString("M")}", cancellationToken: cancellationToken);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return _nextHandler.HandleErrorAsync(botClient, exception, cancellationToken);
    }
}