using Telegram.Bot;

namespace EscaladaBot.Contracts;

public interface IProblemViewer
{
    Task ViewProblem(ITelegramBotClient botClient, IReadOnlyCollection<long> chatIds, int problemId, bool isNewProblem);
}