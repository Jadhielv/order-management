using System.Data;
using Dapper;
using OrderManagement.Data.Context;

namespace OrderManagement.Data.Database
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDatabaseContext _context;

        public DbConnectionFactory(IDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<T>(sql, param);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public async Task<int> ExecuteAsync(string sql, object? param = null)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(sql, param);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object? param = null)
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<T>(sql, param);
        }
    }
} 