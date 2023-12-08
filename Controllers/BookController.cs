using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookReservationSystem.Models;

namespace BookReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly BookContext _context;

        public BookController(ILogger<BookController> logger, BookContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetBookReservation")]
        public async Task<IEnumerable<Book>> Get()
        {
            return await _context.Books.ToListAsync();
        }

        [HttpGet("{id}", Name = "GetBookReservationById")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
        [HttpPost(Name = "PostBook")]
        public async Task<ActionResult<string>> PostBook(Book book)
        {
            // check if the book data is valid
            if (!BookDataCheck(book))
            {
                return BadRequest(new { message = "Book data is not valid." });
            }
            // check if the book already exists
            if (_context.Books.Any(b => b.Id == book.Id))
            {
                return Conflict(new { message = "A book with the same ID already exists." });
            }
            // add the book to the database
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Book added successfully with ID: {book.Id} and Title: {book.Title}." });
        }

        // private bool method to check mendatory fields
        private bool BookDataCheck(Book book)
        {
            if (book.Id < 0)
            {
                return false;
            }
            if (book.Title == null || book.Title == "")
            {
                return false;
            }
            return true;
        }
 
        //update book data
        [HttpPut("{id}", Name = "PutBook")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            // check if the book data is valid
            if (id != book.Id)
            {
                return BadRequest();
            }
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = $"Book updated successfully with ID: {book.Id} and Title: {book.Title}." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        [HttpDelete("{id}", Name = "DeleteBook")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}