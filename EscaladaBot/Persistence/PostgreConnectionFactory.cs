using System.Data;
using EscaladaBot.Contracts;
using Npgsql;

namespace EscaladaBot.Persistence;

public class PostgreConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;
    
    public PostgreConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}