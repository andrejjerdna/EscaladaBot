using System.Data;
using EscaladaBot.Contracts;
using Npgsql;

namespace EscaladaBot.Persistence;

public class PostgresConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;
    
    public PostgresConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}