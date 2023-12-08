using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookReservationSystem.Models;

namespace BookReservationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookReserveController : ControllerBase
    {
        private readonly ILogger<BookReserveController> _logger;
        private readonly BookContext _context;

        public BookReserveController(ILogger<BookReserveController> logger, BookContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Reserve the book by passing the id and the comment if the book already reserved or book does not exists then retrun conflict
        // POST: api/BookReserve
        [HttpPost("ReserveBook")]
        public async Task<ActionResult<string>> ReserveBook(int id, string comment)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            if (book.IsReserved)
            {
                return Conflict(new { message = "The book is already reserved." });
            }
            book.IsReserved = true;
            book.ReservationComment = comment;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Book reserved successfully with ID: {book.Id} and Title: {book.Title}." });
        }
        //POST endpoint to remove reserved status for a book
        //Book id should be passed as a parameter
        [HttpPost("RemoveReservation")]
        public async Task<ActionResult<string>> RemoveReservation(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            if (!book.IsReserved)
            {
                return Conflict(new { message = "The book is not reserved." });
            }
            book.IsReserved = false;
            book.ReservationComment = null;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Reservation removed successfully for book with ID: {book.Id} and Title: {book.Title}." });
        }
        // GET endpoint to get list of reserved books with reservation comment
        // GET: api/BookReserve
        [HttpGet("GetReservedBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetReservedBooks()
        {
            // check if there are no reserved books in the database
            if (!_context.Books.Any(b => b.IsReserved))
            {
                return NoContent();
            }
            return await _context.Books.Where(b => b.IsReserved).ToListAsync();
        }
        
        // GET endpoint to get list of available (not reserved) books
        // GET: api/BookReserve
        [HttpGet("GetAvailableBooks")]
        public async Task<IEnumerable<Book>> GetAvailableBooks()
        {
            return await _context.Books.Where(b => !b.IsReserved).ToListAsync();
        }
        
    }
}