using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LearnAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false),
                    YearPublished = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Title", "YearPublished" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", "The Great Gatsby", 1925 },
                    { 2, "Harper Lee", "To Kill a Mockingbird", 1960 },
                    { 3, "George Orwell", "1984", 1949 },
                    { 4, "Jane Austen", "Pride and Prejudice", 1813 },
                    { 5, "Herman Melville", "Moby-Dick", 1851 },
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
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
