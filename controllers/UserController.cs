using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Database.DTOs;


namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        [AllowAnonymous] //temporary

        public async Task<ActionResult<IEnumerable<ReadUserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Books)
                .Select(u => new ReadUserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Books = u.Books.Select(b => new ReadBookDTO
                    {
                        Title = b.Title,
                        Author = b.Author
                    }).ToList()
                })
                .ToListAsync();

            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadUserDTO>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Books)
                .Where(u => u.Id == id)
                .Select(u => new ReadUserDTO
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    Books = u.Books.Select(b => new ReadBookDTO
                    {
                        Title = b.Title,
                        Author = b.Author
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ReadUserDTO>> PostUser(CreateUserDTO dto)
        {
            try
            {
                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var readUserDto = new ReadUserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    Books = new List<ReadBookDTO>() // No books at creation
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, readUserDto);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("IX_Users_Email") == true)
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UpdateUserDTO dto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user.Email = dto.Email;
            }
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                user.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
