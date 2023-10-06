using Dapper;
using EscaladaBot.Contracts;
using Microsoft.Data.Sqlite;

namespace EscaladaBot.Persistence;

public sealed class RepositoryFactory : IRepositoryFactory
{
    private readonly ISQLiteConnectionFactory _connectionFactory;

    public RepositoryFactory(ISQLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IRepositoryFactory> Build()
    {
        SqliteConnection connection = null;
        try
        {
            connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(
                @"CREATE TABLE IF NOT EXISTS problem_creator_state
(
    chat_id BIGINT UNIQUE NOT NULL PRIMARY KEY,
    trace_creator_state INTEGER NOT NULL,
    problem_id INTEGER NOT NULL,
    update_at DATETIME
);

CREATE TABLE IF NOT EXISTS problem
(
    id BIGINT NOT NULL PRIMARY KEY,
    file_id UUID NOT NULL,
    author VARCHAR(150) NOT NULL,
    timestamp DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS subscribe_user
(
    chat_id BIGINT UNIQUE NOT NULL PRIMARY KEY,
    user_name VARCHAR(300)
);

CREATE TABLE IF NOT EXISTS admin
(
    chat_id BIGINT UNIQUE NOT NULL PRIMARY KEY,
    user_name VARCHAR(300)
);");
        }
        catch(Exception e)
        {
            throw new Exception();
        }
        finally
        {
            await connection?.CloseAsync();
        }

        return new RepositoryFactory(_connectionFactory);
    }
}