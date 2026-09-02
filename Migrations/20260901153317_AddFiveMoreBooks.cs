using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFiveMoreBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Title", "YearPublished" },
                values: new object[,]
                {
                    { 6, "KF Monkwe", "C# Unlocked", 2026 },
                    { 7, "Robert C. Martin", "Clean Code", 2008 },
                    { 8, "James Clear", "Atomic Habits", 2018 },
                    { 9, "Trevor Noah", "Born a Crime", 2016 },
                    { 10, "John Buchan", "The Thirty-Nine Steps", 1915 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
