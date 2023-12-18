using EscaladaBot.Contracts;
using EscaladaBot.Services.Helpers;
using EscaladaBot.Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services;

public sealed class ProblemViewer : IProblemViewer
{
    private readonly IProblemRepository _repository;
    private readonly IFileStore _fileStore;

    public ProblemViewer(IProblemRepository repository, IFileStore fileStore)
    {
        _repository = repository;
        _fileStore = fileStore;
    }

    public async Task ViewProblem(ITelegramBotClient botClient, 
        IReadOnlyCollection<long> chatIds, 
        int problemId,
        bool isNewProblem)
    {
        var problem = await _repository.GetTrace(problemId);

        if (problem == null)
            return;
        
        var files = await _fileStore.GetFiles(problem.FileId);

        var fileId = string.Empty;
        
        foreach (var file in files)
        {
            foreach (var chatId in chatIds)
            {
                if (!string.IsNullOrEmpty(fileId))
                {
                    await botClient.SendPhotoAsync(
                        chatId: chatId,
                        photo: InputFile.FromFileId(fileId),
                        caption: GetCaption(problem, isNewProblem));
                    
                    continue;
                }
                
                var message = await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromStream(stream: file),
                    caption: GetCaption(problem, isNewProblem));

                fileId = message.Photo?.MaxBy(p => p.FileSize)?.FileId;
            }
        }
    }
    
    private static string GetCaption(Problem problem, bool isNewProblem)
    {
        return isNewProblem ? "У нас новая трасса!" : "Отличный выбор! Пролезь её прямо сейчас!";
    }
}