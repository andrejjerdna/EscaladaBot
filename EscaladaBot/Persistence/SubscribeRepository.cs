using Dapper;
using EscaladaBot.Contracts;
using Microsoft.Extensions.Logging;

namespace EscaladaBot.Persistence;

public class SubscribeRepository : ISubscribeRepository
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<SubscribeRepository> _logger;

    public SubscribeRepository(IConnectionFactory connectionFactory, ILogger<SubscribeRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task Subscribe(long chatId, string user)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            var id = await connection.ExecuteAsync(
                @"INSERT INTO subscribe_user (chat_id, user_name) 
                        VALUES (@chatId, @user) 
                        ON CONFLICT (chat_id) DO NOTHING", new {chatId, user});
        }
        catch (Exception e)
        {
            _logger.LogWarning("Exception: {0}", e.Message);
            throw;
        }
    }

    public async Task Unsubscribe(long chatId)
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            var id = await connection.ExecuteAsync(
                @"DELETE FROM subscribe_user WHERE chat_id = @chatId", new {chatId});
        }
        catch (Exception e)
        {
            _logger.LogWarning("Exception: {0}", e.Message);
            throw;
        }
    }

    public async Task<IReadOnlyCollection<long>> GetAll()
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            return (await connection.QueryAsync<long>(
                @"SELECT chat_id FROM subscribe_user")).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogWarning("Exception: {0}", e.Message);
            throw;
        }
    }
}