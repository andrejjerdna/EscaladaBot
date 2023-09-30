using EscaladaBot.Contracts;
using Npgsql;

namespace EscaladaBot.Persistence;

public class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly string _connectionString;
    
    public PostgresConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}