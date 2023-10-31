namespace EscaladaBot.Contracts;

public interface IStatisticsRepository
{
    Task AddVoice(long problemId, long chatId);
    Task<IReadOnlyCollection<(long problemId, int voiceCount)>> GetPopularProblems(int count);
}