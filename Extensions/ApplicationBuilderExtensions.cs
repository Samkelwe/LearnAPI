
using LearnAPI.Middleware;

namespace LearnAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        // Configure request logging
        public static IApplicationBuilder ConfigureRequestLogging(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }

        // Configure Swagger
        public static IApplicationBuilder ConfigureSwagger(
            this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }

        // Configure application middleware
        public static IApplicationBuilder ConfigureApplicationMiddleware(
            this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
