using Dapper;
using EscaladaBot.Contracts;
using EscaladaBot.Services;
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
                @"INSERT INTO problem (author) VALUES (@author) RETURNING id",
                new { request.Author });
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

    public async Task SetDescription(long id, string description)
    {
        SqliteConnection connection = null;
        try
        {
            connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(
                @"UPDATE problem SET description = @description WHERE id = @id",
                new { id, description });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }
}