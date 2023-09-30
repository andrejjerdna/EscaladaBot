using Dapper;
using EscaladaBot.Contracts;
using EscaladaBot.Services.Models;
using Microsoft.Data.Sqlite;

namespace EscaladaBot.Persistence;

public class ProblemRepository : IProblemRepository
{
    private readonly ISQLiteConnectionFactory _connectionFactory;

    public ProblemRepository(ISQLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> AddTrace(CreateTraceRequest request)
    {
        SqliteConnection connection = null;
        try
        {
            connection = _connectionFactory.GetConnection();

            return await connection.QuerySingleAsync<int>(
                @"INSERT INTO problem (author, file_id, timeStamp) 
                        VALUES (@author, @fileId, @timeStamp) 
                        RETURNING id",
                new
                {
                    author = request.Author, 
                    fileId = request.FileId.ToString(), 
                    timeStamp = request.TimeStamp
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }

    public async Task<Problem> GetTrace(int problemId)
    {
        SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
        
        SqliteConnection connection = null;
        try
        {
            connection = _connectionFactory.GetConnection();

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
            Console.WriteLine(e);
            return null;
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }
}