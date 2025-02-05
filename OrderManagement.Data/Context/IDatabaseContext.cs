using System.Data;

namespace OrderManagement.Data.Context
{
    public interface IDatabaseContext
    {
        IDbConnection CreateConnection();
    }
} 