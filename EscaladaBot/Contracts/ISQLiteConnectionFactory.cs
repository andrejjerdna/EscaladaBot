using Microsoft.Data.Sqlite;
using Npgsql;

namespace EscaladaBot.Contracts;

public interface ISQLiteConnectionFactory
{
    SqliteConnection GetConnection();
}