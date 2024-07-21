using HotelManagementSystem.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelManagementSystem.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, $"Error: {exception.Message}");

            // Create custom response
            var details = new ProblemDetails()
            {
                Type = exception.GetType().Name,
                Detail = exception.Message
            };
            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";

            // Set status code for ApiException
            if (exception is ApiException)
            {
                var ex = exception as ApiException;
                httpContext.Response.StatusCode = (int)ex.StatusCode;
            }

            // Add the custom response to http response
            await httpContext.Response.WriteAsync(response, cancellationToken);
            return true;
        }
    }
}
