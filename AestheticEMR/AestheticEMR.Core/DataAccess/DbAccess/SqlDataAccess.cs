using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DbAccess;

public class SqlDataAccess(IConfiguration config) : ISqlDataAccess
{
    public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId)
    {
        using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionId));

        return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<T>> LoadDataText<T, U>(string query, U parameters, string connectionId)
    {
        using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionId))
            ?? throw new InvalidOperationException($"Connection string '{connectionId}' was not found.");

        var normalizedQuery = NormalizeQuery(query);
        return await connection.QueryAsync<T>(normalizedQuery, parameters, commandType: CommandType.Text);
    }

    public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId)
    {
        using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionId));

        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task SaveDataText<T>(string query, T parameters, string connectionId)
    {
        using IDbConnection connection = new SqlConnection(config.GetConnectionString(connectionId));

        var normalizedQuery = NormalizeQuery(query);
        await connection.ExecuteAsync(normalizedQuery, parameters, commandType: CommandType.Text);
    }

    private static string NormalizeQuery(string query)
    {
        return query.Replace(";_", ";", StringComparison.Ordinal);
    }
}
