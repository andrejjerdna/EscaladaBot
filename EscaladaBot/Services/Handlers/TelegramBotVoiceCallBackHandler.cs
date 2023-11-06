using EscaladaBot.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.Handlers;

public sealed class TelegramBotVoiceCallBackHandler : ITelegramBotHandler
{
    private readonly ITelegramBotHandler _nextHandler;
    private readonly IVoiceRepository _voiceRepository;
    private readonly IStatisticsRepository _statisticsRepository;

    public TelegramBotVoiceCallBackHandler(ITelegramBotHandler nextHandler, 
        IVoiceRepository voiceRepository, 
        IStatisticsRepository statisticsRepository)
    {
        _nextHandler = nextHandler;
        _voiceRepository = voiceRepository;
        _statisticsRepository = statisticsRepository;
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

        var model = _voiceRepository.GetVoiceModel(guid);

        if (model == null)
        {
            await _nextHandler.HandleUpdateAsync(botClient, update, cancellationToken);
            return;
        }

        await _statisticsRepository.AddVoice(model.ProblemId, model.ChatId);

        await botClient.SendTextMessageAsync(
            model.ChatId,
            $"Спасибо! Ты отметил трассу {model.ProblemId}!",
            cancellationToken: cancellationToken);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        return _nextHandler.HandleErrorAsync(botClient, exception, cancellationToken);
    }
}