using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class AddImagesStartCommand : IBotCommand
{
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;

    public AddImagesStartCommand(IProblemCreatorStateStore problemCreatorStateStore)
    {
        _problemCreatorStateStore = problemCreatorStateStore;
    }

    public string Name => nameof(AddImagesStartCommand);
    
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;
        
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: $"Отправь фото трассы.");
        
        await _problemCreatorStateStore.CommitState(chatId.Value);

        return true;
    }

    public async Task NotCompleted(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;
        
        await _problemCreatorStateStore.Remove(chatId.Value);

        await botClient.SendMessage(chatId.Value, 
            "Загрузка фото завершилась неудачей. Начни процесс создания трассы сначала.");
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return;

        await botClient.SendErrorMessage(chatId.Value);
    }
}