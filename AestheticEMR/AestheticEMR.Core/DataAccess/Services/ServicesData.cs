using DataAccess.DbAccess;

namespace DataAccess.Services;

public class ServicesData<T>(ISqlDataAccess db) : IServicesData<T>
{
    public Task<IEnumerable<T>> GetAll(string sql, string commandType, dynamic param, string connectionId) =>
        commandType == "sproc"
            ? db.LoadData<T, dynamic>(sql, param, connectionId)
            : db.LoadDataText<T, dynamic>(sql, param, connectionId);

    public async Task<T?> GetById<Q>(string sql, Q id, string commandType, dynamic param, string connectionId)
    {
        var results = commandType == "sproc"
            ? (await db.LoadData<T, dynamic>(sql, param, connectionId)).ToList()
            : (await db.LoadDataText<T, dynamic>(sql, param, connectionId)).ToList();

        return results.FirstOrDefault();
    }

    public Task Insert(string sql, T entity, string commandType, dynamic param, string connectionId) =>
        commandType == "sproc"
            ? db.SaveData(sql, param, connectionId)
            : db.SaveDataText(sql, param, connectionId);

    public Task Update(string sql, T entity, string commandType, dynamic param, string connectionId) =>
        commandType == "sproc"
            ? db.SaveData(sql, entity, connectionId)
            : db.SaveDataText(sql, entity, connectionId);

    public Task Delete<Q>(string sql, Q id, string commandType, dynamic param, string connectionId) =>
        commandType == "sproc"
            ? db.SaveData(sql, param, connectionId)
            : db.SaveDataText(sql, param, connectionId);
}
