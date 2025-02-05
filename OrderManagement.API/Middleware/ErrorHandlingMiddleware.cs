using System.Net;
using System.Text.Json;
using OrderManagement.Core.Exceptions;

namespace OrderManagement.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new
                {
                    Message = error.Message,
                    StatusCode = GetStatusCode(error)
                };

                switch (error)
                {
                    case OrderManagementException e:
                        response.StatusCode = e.StatusCode;
                        break;
                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ArgumentException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        _logger.LogError(error, "An unexpected error occurred");
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorResponse = new
                        {
                            Message = "An unexpected error occurred",
                            StatusCode = response.StatusCode
                        };
                        break;
                }

                var result = JsonSerializer.Serialize(errorResponse);
                await response.WriteAsync(result);
            }
        }

        private static int GetStatusCode(Exception error)
        {
            return error switch
            {
                OrderManagementException e => e.StatusCode,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 