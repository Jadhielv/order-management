namespace OrderManagement.Core.Exceptions
{
    public class OrderManagementException : Exception
    {
        public int StatusCode { get; }

        public OrderManagementException(string message, int statusCode = 500) 
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
} 