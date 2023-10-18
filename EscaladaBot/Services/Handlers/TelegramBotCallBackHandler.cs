using EscaladaBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.Handlers;

public sealed class TelegramBotCallBackHandler : ITelegramBotHandler
{
    private readonly ITelegramBotHandler _nextHandler;
    private readonly IRegisterRepository _registerRepository;
    private readonly IAdminRepository _adminRepository;

    public TelegramBotCallBackHandler(ITelegramBotHandler nextHandler, 
        IRegisterRepository registerRepository, 
        IAdminRepository adminRepository)
    {
        _nextHandler = nextHandler;
        _registerRepository = registerRepository;
        _adminRepository = adminRepository;
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

        var admins = await _adminRepository.GetAdmins();

        foreach (var admin in admins)
        {
            await botClient.SendTextMessageAsync(
                chatId: admin,
                text: $"{model.User} {GetUserLogin(model.Login)} хочет прийти {model.DateTime.ToString("M")}",
                cancellationToken: cancellationToken);
        }

        await botClient.SendTextMessageAsync(
            model.ChatId,
            $"Я сообщил о том, что ты хочешь прийти! Если есть какие-то вопросы пиши @andrejjerdna",
            cancellationToken: cancellationToken);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return _nextHandler.HandleErrorAsync(botClient, exception, cancellationToken);
    }

    private static string GetUserLogin(string login)
    {
        return string.IsNullOrWhiteSpace(login) ? string.Empty : $"(@{login})";
    }
}