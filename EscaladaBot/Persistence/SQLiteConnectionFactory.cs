using EscaladaBot.Contracts;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace EscaladaBot.Persistence;

public class SQLiteConnectionFactory : ISQLiteConnectionFactory
{
    private readonly string _connectionString;
    
    public SQLiteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}