namespace DataAccess.DbAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId);
    Task<IEnumerable<T>> LoadDataText<T, U>(string query, U parameters, string connectionId);

    Task SaveData<T>(string storedProcedure, T parameters, string connectionId);
    Task SaveDataText<T>(string query, T parameters, string connectionId);
}
