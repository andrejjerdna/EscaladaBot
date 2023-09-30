using Dapper;
using EscaladaBot.Contracts;
using Microsoft.Data.Sqlite;

namespace EscaladaBot.Persistence;

public class SubscribeRepository : ISubscribeRepository
{
    private readonly ISQLiteConnectionFactory _connectionFactory;

    public SubscribeRepository(ISQLiteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Subscribe(long chatId, string user)
    {
        SqliteConnection connection = null;
        
        try
        {
            connection = _connectionFactory.GetConnection();

            var id = await connection.ExecuteAsync(
                @"INSERT INTO subscribe_user (chat_id, user_name) 
                        VALUES (@chatId, @user)", new {chatId, user});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }

    public async Task Unsubscribe(long chatId)
    {
        SqliteConnection connection = null;
        
        try
        {
            connection = _connectionFactory.GetConnection();

            var id = await connection.ExecuteAsync(
                @"DELETE FROM subscribe_user WHERE chat_id = @chatId", new {chatId});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }

    public async Task<IReadOnlyCollection<long>> GetAll()
    {
        SqliteConnection connection = null;
        
        try
        {
            connection = _connectionFactory.GetConnection();

            return (await connection.QueryAsync<long>(
                @"SELECT chat_id FROM subscribe_user")).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection?.CloseAsync();
        }
    }
}