using System.Data;
using Dapper;
using EscaladaBot.Contracts;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace EscaladaBot.Persistence;

public sealed class AdminRepository : IAdminRepository
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<AdminRepository> _logger;

    public AdminRepository(IConnectionFactory connectionFactory, ILogger<AdminRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<long>> GetAdmins()
    {
        try
        {
            var connection = _connectionFactory.GetConnection();

            return (await connection.QueryAsync<long>(
                @"SELECT chat_id FROM admin")).ToArray();
        }
        catch (Exception e)
        {
            _logger.LogWarning("List of admins was not found");
            return ArraySegment<long>.Empty;
        }
    }
}