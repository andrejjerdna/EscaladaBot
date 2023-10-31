using Dapper;
using EscaladaBot.Contracts;

namespace EscaladaBot.Persistence;

public sealed class RepositoryFactory : IRepositoryFactory
{
    private readonly IConnectionFactory _connectionFactory;

    public RepositoryFactory(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IRepositoryFactory> Build()
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            await connection.ExecuteAsync(
                @"
CREATE TABLE IF NOT EXISTS problem_creator_state
(
    chat_id BIGINT UNIQUE NOT NULL PRIMARY KEY,
    trace_creator_state INTEGER NOT NULL,
    problem_id INTEGER NOT NULL,
    update_at TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE TABLE IF NOT EXISTS problem
(
    id BIGSERIAL NOT NULL PRIMARY KEY,
    file_id UUID NOT NULL,
    author VARCHAR(150) NOT NULL,
    timestamp TIMESTAMP WITH TIME ZONE NOT NULL
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
);

CREATE TABLE IF NOT EXISTS problem_statistics
(
    problem_id BIGINT NOT NULL,
    chat_id BIGINT NOT NULL,
    timestamp TIMESTAMP WITH TIME ZONE NOT NULL
);");
        }
        catch(Exception e)
        {
            throw new Exception();
        }

        return new RepositoryFactory(_connectionFactory);
    }
}