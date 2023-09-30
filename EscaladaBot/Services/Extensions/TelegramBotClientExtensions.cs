using Telegram.Bot;

namespace EscaladaBot.Services.Extensions;

public static class TelegramBotClientExtensions
{
    public static async Task SendErrorMessage(this ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"К сожалению создать трассу не удалось((( Если не получитcя еще раз, напиши: @andrejjerdna");
    }
    
    public static async Task SendMessage(this ITelegramBotClient botClient, long chatId, string message)
    {
        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message);
    }
}