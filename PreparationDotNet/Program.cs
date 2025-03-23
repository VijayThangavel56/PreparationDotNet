using PreparationDotNet.Middleware;
using Scalar.AspNetCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddTransient<NeExceptionMiddleware>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("My API");
        options.WithTheme(ScalarTheme.BluePlanet);
        options.WithSidebar(true);
    });
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
    });
}
app.Services.GetRequiredService<ILogger<Program>>(); // Specify the logger type (ILogger<Program>)

//app.ConfigureExceptionHandler(logger);
//app.ConfigureCustomExceptionMiddleware();
app.UseMiddleware<NeExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
