
using LearnAPI.Extensions;
using LearnAPI.Middleware;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
LogManager.Setup().LoadConfigurationFromFile(
    string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Register application services
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureJwtAuthentication(builder.Configuration);
builder.Services.ConfigureSwaggerServices();

var app = builder.Build();

// Configure middleware
app.ConfigureExceptionHandler();
app.ConfigureRequestLogging();
app.ConfigureSwagger();
app.ConfigureApplicationMiddleware();

// Apply database migrations
app.ApplyDatabaseMigrations();

// Map endpoints
app.MapControllers();
app.MapGet("/", () => "API is running");

// Configure hosting port
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.Run();
