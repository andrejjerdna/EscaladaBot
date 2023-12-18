using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class EndCommand : IBotCommand
{
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;

    public EndCommand(IProblemCreatorStateStore problemCreatorStateStore)
    {
        _problemCreatorStateStore = problemCreatorStateStore;
    }

    public string Name => nameof(EndCommand);
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;
        
        await _problemCreatorStateStore.Remove(chatId.Value);
       
        return true;
    }

    public async Task NotCompleted(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
        
        await _problemCreatorStateStore.Remove(chatId.Value);

        await botClient.SendMessage(chatId.Value, 
            "Сохранение трассы завершилось неудачей. Начни процесс создания трассы сначала.");
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
        
        await _problemCreatorStateStore.Remove(chatId.Value);
        
        await botClient.SendErrorMessage(chatId.Value);
    }
}