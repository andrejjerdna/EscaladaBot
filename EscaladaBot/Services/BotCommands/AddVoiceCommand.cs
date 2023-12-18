using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using EscaladaBot.Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EscaladaBot.Services.BotCommands;

public sealed class AddVoiceCommand : IBotCommand
{
    private readonly IProblemRepository _problemRepository;
    private readonly IVoiceRepository _voiceRepository;

    public AddVoiceCommand(IProblemRepository problemRepository, 
        IVoiceRepository voiceRepository)
    {
        _problemRepository = problemRepository;
        _voiceRepository = voiceRepository;
    }

    public string Name => nameof(AddVoiceCommand);
    
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;

        var problems = await _problemRepository.GetAllProblems();

        var inlineKeyboard = new InlineKeyboardMarkup(problems
            .Select(d
                => InlineKeyboardButton.WithCallbackData(
                    text: d.Id.ToString(),
                    callbackData: GetCommandDate(chatId.Value, d.Id)))
            .Chunk(6)
            .ToArray());
        
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери трассы которые пролез сегодня или которые пробовал",
            replyMarkup: inlineKeyboard,
            cancellationToken: CancellationToken.None);

        return true;
    }

    public Task NotCompleted(ITelegramBotClient botClient, Update update)
    {
        return Task.CompletedTask;
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
       
        await botClient.SendErrorMessage(chatId.Value);
    }
    
    private string GetCommandDate(long chatId, long problemId)
    {
        var guid = _voiceRepository.AddVoice(new AddVoiceModel(chatId, problemId));
        return guid.ToString();
    }
}