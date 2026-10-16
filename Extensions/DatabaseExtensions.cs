
using LearnAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ApplyDatabaseMigrations(
            this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<BookContext>();

            Console.WriteLine("Running migrations...");
            db.Database.Migrate();
            Console.WriteLine(
                "Migrations done - RefreshTokens table should now exist");
        }
    }
}
