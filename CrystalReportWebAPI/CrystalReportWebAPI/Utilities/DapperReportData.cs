using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CrystalReportWebAPI.Utilities
{
    public static class DapperReportData
    {
        public static async Task<DataSet> ExecuteDataSetAsync(string connectionString, string storedProcedure, object parameters, int commandTimeout = 240)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                return await ExecuteDataSetAsync(conn, storedProcedure, parameters, commandTimeout);
            }
        }

        public static async Task<DataSet> ExecuteDataSetAsync(SqlConnection connection, string storedProcedure, object parameters, int commandTimeout = 240)
        {
            var ds = new DataSet();
            using (var reader = await connection.ExecuteReaderAsync(new CommandDefinition(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout)))
            {
                var dt = new DataTable();
                dt.Load(reader);
                ds.Tables.Add(dt);
            }

            return ds;
        }
    }
}
