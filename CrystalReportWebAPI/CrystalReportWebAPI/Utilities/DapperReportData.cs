using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CrystalReportWebAPI.Utilities
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionString, int commandTimeout = 240);
        Task<IEnumerable<T>> LoadDataText<T, U>(string query, U parameters, string connectionString, int commandTimeout = 240);
        Task SaveData<T>(string storedProcedure, T parameters, string connectionString, int commandTimeout = 240);
        Task SaveDataText<T>(string query, T parameters, string connectionString, int commandTimeout = 240);
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionString, int commandTimeout = 240)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }

        public async Task<IEnumerable<T>> LoadDataText<T, U>(string query, U parameters, string connectionString, int commandTimeout = 240)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<T>(NormalizeQuery(query), parameters, commandType: CommandType.Text, commandTimeout: commandTimeout);
            }
        }

        public async Task SaveData<T>(string storedProcedure, T parameters, string connectionString, int commandTimeout = 240)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            }
        }

        public async Task SaveDataText<T>(string query, T parameters, string connectionString, int commandTimeout = 240)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(NormalizeQuery(query), parameters, commandType: CommandType.Text, commandTimeout: commandTimeout);
            }
        }

        private static string NormalizeQuery(string query)
        {
            return query.Replace(";_", ";");
        }
    }

    public interface IServicesData<T>
    {
        Task<IEnumerable<T>> GetAll(string sql, string commandType, object param, string connectionString, int commandTimeout = 240);
        Task<T> GetById<Q>(string sql, Q id, string commandType, object param, string connectionString, int commandTimeout = 240);
        Task Insert(string sql, T entity, string commandType, object param, string connectionString, int commandTimeout = 240);
        Task Update(string sql, T entity, string commandType, object param, string connectionString, int commandTimeout = 240);
        Task Delete<Q>(string sql, Q id, string commandType, object param, string connectionString, int commandTimeout = 240);
    }

    public class ServicesData<T> : IServicesData<T>
    {
        private readonly ISqlDataAccess db;

        public ServicesData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public Task<IEnumerable<T>> GetAll(string sql, string commandType, object param, string connectionString, int commandTimeout = 240)
        {
            return commandType == "sproc"
                ? db.LoadData<T, object>(sql, param, connectionString, commandTimeout)
                : db.LoadDataText<T, object>(sql, param, connectionString, commandTimeout);
        }

        public async Task<T> GetById<Q>(string sql, Q id, string commandType, object param, string connectionString, int commandTimeout = 240)
        {
            var results = commandType == "sproc"
                ? (await db.LoadData<T, object>(sql, param, connectionString, commandTimeout)).ToList()
                : (await db.LoadDataText<T, object>(sql, param, connectionString, commandTimeout)).ToList();

            return results.FirstOrDefault();
        }

        public Task Insert(string sql, T entity, string commandType, object param, string connectionString, int commandTimeout = 240)
        {
            return commandType == "sproc"
                ? db.SaveData(sql, param, connectionString, commandTimeout)
                : db.SaveDataText(sql, param, connectionString, commandTimeout);
        }

        public Task Update(string sql, T entity, string commandType, object param, string connectionString, int commandTimeout = 240)
        {
            return commandType == "sproc"
                ? db.SaveData(sql, entity, connectionString, commandTimeout)
                : db.SaveDataText(sql, entity, connectionString, commandTimeout);
        }

        public Task Delete<Q>(string sql, Q id, string commandType, object param, string connectionString, int commandTimeout = 240)
        {
            return commandType == "sproc"
                ? db.SaveData(sql, param, connectionString, commandTimeout)
                : db.SaveDataText(sql, param, connectionString, commandTimeout);
        }
    }

    public static class DapperReportData
    {
        private static readonly IServicesData<dynamic> ServiceData = new ServicesData<dynamic>(new SqlDataAccess());

        public static async Task<DataSet> ExecuteDataSetAsync(string connectionString, string command, object parameters, int commandTimeout = 240)
        {
            var commandType = IsSqlText(command) ? "text" : "sproc";
            var rows = await ServiceData.GetAll(command, commandType, parameters, connectionString, commandTimeout);
            return ToDataSet(rows);
        }

        public static Task<DataSet> ExecuteDataSetAsync(SqlConnection connection, string command, object parameters, int commandTimeout = 240)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return ExecuteDataSetAsync(connection.ConnectionString, command, parameters, commandTimeout);
        }

        private static bool IsSqlText(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentException("Command cannot be empty.", "command");
            }

            var sql = command.TrimStart();
            return sql.Contains("\n") || sql.Contains("\r") || sql.Contains(" ");
        }

        private static DataSet ToDataSet(IEnumerable<dynamic> rows)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            var dictionaries = rows == null
                ? new List<IDictionary<string, object>>()
                : rows.Select(r => r as IDictionary<string, object> ?? new Dictionary<string, object>()).ToList();

            if (dictionaries.Count == 0)
            {
                dataSet.Tables.Add(dataTable);
                return dataSet;
            }

            var columnNames = new List<string>();
            foreach (var key in dictionaries[0].Keys)
            {
                columnNames.Add(key);
            }

            foreach (var rowDict in dictionaries)
            {
                foreach (var key in rowDict.Keys)
                {
                    if (!columnNames.Contains(key))
                    {
                        columnNames.Add(key);
                    }
                }
            }

            foreach (var columnName in columnNames)
            {
                dataTable.Columns.Add(columnName, GetColumnType(dictionaries, columnName));
            }

            foreach (var rowDict in dictionaries)
            {
                var row = dataTable.NewRow();
                foreach (var columnName in columnNames)
                {
                    object value;
                    if (!rowDict.TryGetValue(columnName, out value) || value == null)
                    {
                        row[columnName] = DBNull.Value;
                        continue;
                    }

                    row[columnName] = value;
                }

                dataTable.Rows.Add(row);
            }

            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        private static Type GetColumnType(IReadOnlyCollection<IDictionary<string, object>> rows, string columnName)
        {
            foreach (var row in rows)
            {
                object value;
                if (row.TryGetValue(columnName, out value) && value != null)
                {
                    return Nullable.GetUnderlyingType(value.GetType()) ?? value.GetType();
                }
            }

            return typeof(string);
        }
    }
}
