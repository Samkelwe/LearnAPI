using LearnAPI.Models;
//using LearnAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        /* static private List<Book> books = new List<Book>
         {
             new Book
             {
                 Id=1,
                 Title= "The Great Gatsby",
                 Author= "F. Scott Fitzgerald",
                 YearPublished=1925
             },
             new Book
             {
                 Id=2,
                 Title= "To Kill a Mockingbird" ,
                 Author= "Harper Lee",
                 YearPublished=1960
             },
             new Book
             {
                 Id=3,
                 Title= "1984" ,
                 Author= "George Orwell",
                 YearPublished=1949
             },
             new Book
             {
                 Id=4,
                 Title= "Pride and Prejudice" ,
                 Author= "Jane Austen",
                 YearPublished=1813
             },
             new Book
             {
                 Id=5,
                 Title= "Moby-Dick" ,
                 Author= "Herman Melville",
                 YearPublished=1851
             }

         }; */

        private readonly BookContext _context;
        public BooksController(BookContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Book>>>GetBooks()
        {
            return Ok(await _context.Books.ToListAsync());
        }
   [HttpGet("{id}")]
     public async Task<ActionResult<Book>>GetBookById(int id)
     {
            var book = await _context.Books.FindAsync(id);
         if (book == null)
             return NotFound();

         return Ok(book);
     }
     [HttpPost]
       public async Task<ActionResult<Book>> AddBook(Book newBook)
       {
           if (newBook == null)
               return BadRequest();

           _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(GetBookById), new {id=newBook.Id},newBook);
       }
       [HttpPut("{id}")]
       public async Task<IActionResult >UpdateBook(int id, Book updatedBook)
       {
           var book = await _context.Books.FindAsync(id);
            if (book == null)
               return NotFound();

           book.Id= updatedBook.Id;
           book.Title= updatedBook.Title;
           book .Author= updatedBook.Author;
           book.YearPublished=updatedBook.YearPublished;

           await _context.SaveChangesAsync();

           return NoContent();
       }
       [HttpDelete("{id}")]
       public async Task<IActionResult >DeleteBook(int id)
      {

           var book = await _context.Books.FindAsync(id);

           if(book == null)
           return NotFound();


           _context.Books.Remove(book);
            await _context.SaveChangesAsync();

           return NoContent();
      }

    }
}
