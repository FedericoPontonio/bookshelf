namespace Database.DTOs
{
    public class CreateBookDTO
    {
        public int UserId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public string? Notes { get; set; }


    }

    public class ReadBookDTO
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public string? Notes { get; set; }


    }

    public class CreateUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class ReadUserDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; } = string.Empty;

        public List<ReadBookDTO>? Books { get; set; }
    }
    public class UpdateUserDTO
    {
        public string? Email { get; set; }
        public string Name { get; set; } = string.Empty;

        public string? Password { get; set; }
    }

}
