using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Database.DTOs;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReadBookDTO>>> GetBooks()
        {
            var books = await _context.Books
                .Select(b => new ReadBookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Image = b.Image,
                    Notes = b.Notes
                })
                .ToListAsync();

            return Ok(books);
        }


        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadBookDTO>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var dto = new ReadBookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Image = book.Image,
                Notes = book.Notes

            };

            return Ok(dto);
        }
        // GET: api/Book/User/5
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<ReadBookDTO>>> GetBooksByUser(int id)
        {
            var books = await _context.Books
                .Where(b => b.UserId == id)
                .Select(b => new ReadBookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Image = b.Image,
                    Notes = b.Notes
                })
                .ToListAsync();

            return Ok(books);
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostUser(CreateBookDTO dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                UserId = dto.UserId,
                Image = dto.Image,
                Notes = dto.Notes
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 response, indicating successful deletion
        }

        // PUT: api/Book/notes/5
        [HttpPut("notes/{id}")]
        public async Task<IActionResult> UpdateBookNotes(int id, [FromBody] string notes)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Notes = notes;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}