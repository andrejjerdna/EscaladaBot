using EscaladaApi.Contracts;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services.BotCommands;

public sealed class AddImagesCommand : IBotCommand
{
    private readonly IProblemCreatorStateStore _problemCreatorStateStore;
    private readonly IFileStore _fileStore;

    public AddImagesCommand(IProblemCreatorStateStore problemCreatorStateStore, IFileStore fileStore)
    {
        _problemCreatorStateStore = problemCreatorStateStore;
        _fileStore = fileStore;
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

        await _problemCreatorStateStore.AddFileFolderId(chatId.Value, folderId);

        var file = await botClient.GetFileAsync(photo.FileId);

        if (file.FilePath == null)
            throw new Exception();
        
        await using var memStream = new MemoryStream();
        
        await botClient.DownloadFileAsync(
            filePath: file.FilePath,
            destination: memStream,
            cancellationToken: CancellationToken.None);

        await _fileStore.SaveFile(memStream, folderId, file.FilePath);

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