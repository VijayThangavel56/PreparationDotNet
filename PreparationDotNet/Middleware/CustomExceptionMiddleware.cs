using PreparationDotNet.Models;
using System.Net;

namespace PreparationDotNet.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue processing the request
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}"); // Log the exception
                await HandleExceptionAsync(context, ex); // Handle the exception
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal server error"
            }.ToString());
        }
    }

    public static class CustomExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>(); // Register custom exception middleware
        }
    }

}
