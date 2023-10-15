using System.Data;
using Microsoft.Data.Sqlite;
using Npgsql;

namespace EscaladaBot.Contracts;

public interface IConnectionFactory
{
    IDbConnection GetConnection();
}