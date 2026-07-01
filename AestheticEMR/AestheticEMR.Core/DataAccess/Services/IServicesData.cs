namespace DataAccess.Services;

public interface IServicesData<T>
{
    Task<IEnumerable<T>> GetAll(string query, string commandType, dynamic param, string connectionId);
    Task<T?> GetById<Q>(string query, Q id, string commandType, dynamic param, string connectionId);
    Task Insert(string query, T entity, string commandType, dynamic param, string connectionId);
    Task Update(string query, T entity, string commandType, dynamic param, string connectionId);
    Task Delete<Q>(string query, Q id, string commandType, dynamic param, string connectionId);
}
