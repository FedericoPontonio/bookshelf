using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // definizioni tabelle
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurazione della relazione uno-a-molti tra User e Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasIndex(b => new { b.Title, b.Author }) // Separate index configuration
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }
        // Navigation property
        public List<Book> Books { get; set; } = new();

    }
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public string? Notes { get; set; }


        public int UserId { get; set; } // Foreign key
        public User User { get; set; } = null!; // Navigation property
    }
}
