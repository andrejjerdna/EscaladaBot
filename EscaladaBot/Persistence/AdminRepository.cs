using Dapper;
using EscaladaBot.Contracts;
using Microsoft.Data.Sqlite;

namespace EscaladaBot.Persistence;

public sealed class AdminRepository : IAdminRepository
{
    private readonly ISQLiteConnectionFactory _connectionFactory;

    public AdminRepository(ISQLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<long>> GetAdmins()
    {
        SqliteConnection connection = null;
        try
        {
            connection = _connectionFactory.GetConnection();

            return (await connection.QueryAsync<long>(
                @"SELECT chat_id FROM admin")).ToArray();
        }
        catch (Exception e)
        {
            return ArraySegment<long>.Empty;
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }
}