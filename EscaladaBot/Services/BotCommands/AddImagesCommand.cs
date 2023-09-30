using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using EscaladaBot.Services.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace EscaladaBot.Services.BotCommands;

public sealed class AddImagesCommand : IBotCommand
{
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;

    public AddImagesCommand(IProblemCreatorStateStore problemCreatorStateStore)
    {
        _problemCreatorStateStore = problemCreatorStateStore;
    }

    public string Name => nameof(AddImagesCommand);
    public async Task<bool> Run(ITelegramBotClient botClient, Update update)
    {
        var chatId = update?.Message?.Chat.Id;
        
        if(!chatId.HasValue)
            return false;
        
        var photo = (update?.Message?.Photo)?.MaxBy(p => p.FileSize);

        if (photo == null)
            throw new Exception();

        var folderId = Guid.NewGuid();

        var newPath = PathHelper.GetFolderPath(folderId.ToString());

        await _problemCreatorStateStore.AddFileFolderId(chatId.Value, folderId);

        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }

        var file = await botClient.GetFileAsync(photo.FileId);
        var ext = Path.GetExtension(file.FilePath);
        var filePath = PathHelper.GetFilePath(newPath, Guid.NewGuid().ToString(), ext);

        if (file.FilePath == null)
            throw new Exception();

        await using Stream fileStream = File.Create(filePath);
        await botClient.DownloadFileAsync(
            filePath: file.FilePath,
            destination: fileStream,
            cancellationToken: CancellationToken.None);
        fileStream.Close();

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
        
        await _problemCreatorStateStore.Remove(chatId.Value);
        
        await botClient.SendErrorMessage(chatId.Value);
    }
}