namespace EscaladaBot.Contracts;

public interface IStatisticsRepository
{
    Task AddVoice(long problemId);
    Task<IReadOnlyCollection<(long problemId, int voiceCount)>> GetPopularProblems(int count);
}