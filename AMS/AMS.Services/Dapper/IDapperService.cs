using Dapper;

namespace AMS.SERVICES.Dapper;

public interface IDapperService : IDisposable
{
    Task<bool> ExecuteAsync(string query, DynamicParameters parameters);
    Task<T> ExecuteScalarAsync<T>(string query, DynamicParameters parameters);
    Task<T> ReturnRowAsync<T>(string query, DynamicParameters parameters);
    Task<IEnumerable<T>> ReturnListAsync<T>(string procedureName, DynamicParameters parameters);
}