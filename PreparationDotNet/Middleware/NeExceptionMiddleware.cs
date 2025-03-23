using Microsoft.AspNetCore.Diagnostics;
using PreparationDotNet.Models;
using System.Text.Json;

namespace PreparationDotNet.Middleware
{
    public class NeExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<NeExceptionMiddleware> _logger;
        public NeExceptionMiddleware(ILogger<NeExceptionMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) {
                _logger.LogError("Internal Server Error");
                await ExceptionHandlerAsync(context, ex);
            }

        }
        public async Task ExceptionHandlerAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var errorDetails = new ErrorDetails()
            {
                StatusCode = (int)StatusCodes.Status500InternalServerError,
                Message = $"Internal server error: {ex.Message}"
            };

            var errorResponse = JsonSerializer.Serialize(errorDetails);

            await context.Response.WriteAsync(errorResponse);
        }
    }
}
