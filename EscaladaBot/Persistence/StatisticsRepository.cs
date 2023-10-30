using Dapper;
using EscaladaBot.Contracts;

namespace EscaladaBot.Persistence;

public sealed class StatisticsRepository : IStatisticsRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public StatisticsRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task AddVoice(long problemId)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(
                @"INSERT INTO public.problem_statistics (problem_id, voice_count)
                    VALUES (@problemId, 1)
                    ON CONFLICT (problem_id)
                    DO UPDATE SET voice_count = problem_statistics.voice_count + 1",
                new { problemId = problemId });
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public async Task<IReadOnlyCollection<(long problemId, int voiceCount)>> GetPopularProblems(int count)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            var result = await connection.QueryAsync<(long problemId, int voiceCount)>(
                @"SELECT problem_id, voice_count 
                FROM public.problem_statistics
                ORDER BY voice_count DESC
                LIMIT @count",
                new { count = count });

            return result.ToArray();
        }
        catch (Exception e)
        {
            return ArraySegment<(long problemId, int voiceCount)>.Empty;
        }
    }
}