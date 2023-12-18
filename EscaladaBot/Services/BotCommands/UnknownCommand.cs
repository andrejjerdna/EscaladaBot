using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class UnknownCommand : IBotCommand
{
    public string Name => nameof(UnknownCommand);
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: $"Не понимаю, что ты хочешь сделать... Попробуй еще раз.");

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