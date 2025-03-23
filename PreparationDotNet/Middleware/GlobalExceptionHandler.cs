
using PreparationDotNet.Models;
using System.Net;
using System.Text.Json;

namespace PreparationDotNet.Middleware
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger= logger;
        }
        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) {
                _logger.LogError($"Internal Server Error");
                await ExceptionHandlerExtensions(context, ex);
            }
        }
        public async Task ExceptionHandlerExtensions(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorDetails = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = $"Internal Server Error {ex.Message}"
            };

            var message = JsonSerializer.Serialize(errorDetails);
            await context.Response.WriteAsync(message);
        }
    }
}
