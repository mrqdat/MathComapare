namespace MathComapare.Entities
{
    public class Users
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? GoogleId { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }

        public List<Games> Games { get; } = [];
        public List<Scores> Scores { get; } = [];
    }
}
