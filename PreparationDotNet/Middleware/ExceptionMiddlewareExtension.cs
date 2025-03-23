using Microsoft.AspNetCore.Diagnostics;
using PreparationDotNet.Models;
using System.Net;

namespace PreparationDotNet.Middleware
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null) {
                        logger.LogError("something error");
                    }
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal server Error"
                    }.ToString());
                });
            });
        }
    }
}
