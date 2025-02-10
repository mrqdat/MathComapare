namespace MathComapare.Entities
{
    public class Games
    {
        public int GameId { get; set; }
        public string? GameName { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }
        public Users? User { get; set; }
    }
}
