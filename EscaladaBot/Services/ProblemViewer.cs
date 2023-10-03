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
        
        foreach (var chatId in chatIds)
        {
            foreach (var file in files)
            {
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromStream(stream: file),
                    caption: GetCaption(problem, isNewProblem));
            }
        }
    }
    
    private static string GetCaption(Problem problem, bool isNewProblem)
    {
        return isNewProblem ? "У нас новая трасса!" : "Отличный выбор! Пролезь её прямо сейчас!";
    }
}