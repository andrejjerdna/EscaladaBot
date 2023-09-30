using Npgsql;

namespace EscaladaBot.Contracts;

public interface ISQLiteConnectionFactory
{
    NpgsqlConnection GetConnection();
}