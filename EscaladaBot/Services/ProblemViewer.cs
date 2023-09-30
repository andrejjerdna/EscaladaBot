using EscaladaBot.Contracts;
using EscaladaBot.Services.Helpers;
using EscaladaBot.Services.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace EscaladaBot.Services;

public sealed class ProblemViewer : IProblemViewer
{
    private readonly IProblemRepository _repository;

    public ProblemViewer(IProblemRepository repository)
    {
        _repository = repository;
    }

    public async Task ViewProblem(ITelegramBotClient botClient, 
        IReadOnlyCollection<long> chatIds, 
        int problemId,
        bool isNewProblem)
    {
        var problem = await _repository.GetTrace(problemId);

        if (problem == null)
            return;
        
        var newPath = PathHelper.GetFolderPath(problem.FileId.ToString());

        if (!Directory.Exists(newPath))
            return;

        var files = Directory.GetFiles(newPath);

        foreach (var chatId in chatIds)
        {
            foreach (var file in files)
            {
                await using Stream stream = System.IO.File.OpenRead(file);
                await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: InputFile.FromStream(stream: stream, fileName: Path.GetFileName(file)),
                    caption: GetCaption(problem, isNewProblem));
            }
        }
    }
    
    private static string GetCaption(Problem problem, bool isNewProblem)
    {
        return isNewProblem ? "У нас новая трасса!" : "Отличный выбор! Пролезь её прямо сейчас!";
    }
}