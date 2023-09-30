using EscaladaApi.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class WelcomeCommand : IBotCommand
{
    public string Name => nameof(WelcomeCommand);
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;
        
        await botClient.SendMessage(chatId.Value, 
            @"Привет! 
Это бот скалодрома Эскалада.
Тут ты можешь посмотреть фото трасс и сообщить о том, что ты хочешь прийти на занятие");

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
}