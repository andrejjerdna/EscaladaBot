using Dapper;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Models;
using Microsoft.Extensions.Logging;

namespace EscaladaBot.Persistence;

public class ProblemRepository : IProblemRepository
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<ProblemRepository> _logger;

    public ProblemRepository(IConnectionFactory connectionFactory, ILogger<ProblemRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<int> AddTrace(CreateTraceRequest request)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            return await connection.QuerySingleAsync<int>(
                @"INSERT INTO problem (author, file_id, timeStamp) 
                        VALUES (@author, @fileId, @timeStamp) 
                        RETURNING id",
                new
                {
                    author = request.Author, 
                    fileId = request.FileId, 
                    timeStamp = request.TimeStamp
                });
        }
        catch (Exception e)
        {
            _logger.LogWarning("Problem has not been added! Exception: {0}", e.Message);
            return -1;
        }
    }

    public async Task<Problem> GetTrace(int problemId)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            return await connection.QueryFirstOrDefaultAsync<Problem>(
                @"SELECT id, author, file_id AS FileId, timeStamp
                        FROM problem
                        WHERE id = @id;",
                new
                {
                    id = problemId, 
                });
        }
        catch (Exception e)
        {
            _logger.LogWarning("Problem not found! Exception: {0}", e.Message);
            return null;
        }
    }

    public async Task<IReadOnlyCollection<Problem>> GetAllProblems()
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            return (await connection.QueryAsync<Problem>(
                @"SELECT id, author, file_id AS FileId, timeStamp
                        FROM problem;")).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Problems not found! Exception: {0}", e.Message);
            return (IReadOnlyCollection<Problem>)Array.Empty<IReadOnlyCollection<Problem>>();
        }
    }
}