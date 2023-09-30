using EscaladaApi.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;


namespace EscaladaApi.Services;

public class AddImagesCommand : IBotCommand
{
    public async Task Start(ITelegramBotClient botClient, Update update)
    {
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: $"Отправь фото трассы.");
    }

    public async Task<bool> Create(ITelegramBotClient botClient, Update update)
    {
        var file = await botClient.GetFileAsync(update.Message.Document.FileId);
        var fileName = file.FileId + "." + file.FilePath.Split('.').Last();

        await using FileStream fileStream = File.OpenWrite("");

        return true;
    }

    public async Task Error(ITelegramBotClient botClient, Update update)
    {
        await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: $"К сожалению добавить фото не удалось((( Если не получитя еще раз, напиши: @andrejjerdna");
    }
}