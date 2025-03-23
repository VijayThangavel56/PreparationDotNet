
using PreparationDotNet.Models;
using System.Text.Json;

namespace PreparationDotNet.Middleware
{
    public class ExceptionHandler1 : IMiddleware
    {
        private readonly ILogger<ExceptionHandler1> _logger;
        async Task IMiddleware.InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error");
                await ExceptionHandlerAsync(context, ex);
            }
        }

        public async Task ExceptionHandlerAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var meg = new ErrorDetails()
            {
                Message = "Internal server error",
                StatusCode = (int)StatusCodes.Status500InternalServerError
            };
            var message = JsonSerializer.Serialize(meg);

            await context.Response.WriteAsync(message);
        }
    }
}
