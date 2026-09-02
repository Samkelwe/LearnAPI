using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(

                new Book
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    YearPublished = 1925
                },
            new Book
            {
                Id = 2,
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                YearPublished = 1960
            },
            new Book
            {
                Id = 3,
                Title = "1984",
                Author = "George Orwell",
                YearPublished = 1949
            },
            new Book
            {
                Id = 4,
                Title = "Pride and Prejudice",
                Author = "Jane Austen",
                YearPublished = 1813
            },
            new Book
            {
                Id = 5,
                Title = "Moby-Dick",
                Author = "Herman Melville",
                YearPublished = 1851
            },
            new Book
            { 
                Id=6,
                Title= "C# Unlocked",
                Author= "KF Monkwe",
                YearPublished= 2026
            },
            new Book
            {
                Id = 7,
                Title = "Clean Code",
                Author = "Robert C. Martin",
                YearPublished = 2008
            },
            new Book
            {
                Id = 8,
                Title = "Atomic Habits",
                Author = "James Clear",
                YearPublished = 2018
            },
            new Book
            {
                Id = 9,
                Title = "Born a Crime",
                Author = "Trevor Noah",
                YearPublished = 2016
            },
            new Book
            {
                Id = 10,
                Title = "The Thirty-Nine Steps",
                Author = "John Buchan",
                YearPublished = 1915
            }

             );
        }

        public DbSet<Book> Books { get; set; }
    }
}