using System.Data;

namespace OrderManagement.Data.Database
{
    public interface IDbConnectionFactory
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null);
        Task<int> ExecuteAsync(string sql, object? param = null);
        Task<T> ExecuteScalarAsync<T>(string sql, object? param = null);
    }
} 