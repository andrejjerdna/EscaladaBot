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

    public async Task AddVoice(long problemId, long chatId)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(
                @"INSERT INTO public.problem_statistics (problem_id, chat_id, timestamp)
                    VALUES (@problemId, @chatId, @timestamp);",
                new { problemId, chatId, timestamp = DateTime.UtcNow });
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
                @"SELECT problem_id, COUNT(*) 
                FROM public.problem_statistics
                GROUP BY problem_id
                ORDER BY COUNT(problem_id) DESC
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