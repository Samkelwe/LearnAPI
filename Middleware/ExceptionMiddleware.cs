
using LearnAPI.Contracts;
using Microsoft.AspNetCore.Http;

namespace LearnAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Something went wrong: {ex}");

                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                httpContext.Response.Clear();
                httpContext.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                httpContext.Response.ContentType =
                    "application/json";

                await httpContext.Response.WriteAsJsonAsync(
                    new { message = "Internal Server Error." });
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
	{
    return app.UseMiddleware<ExceptionMiddleware>();
	}
    }
}
