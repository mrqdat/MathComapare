namespace MathComapare.Entities
{
    public class Scores
    {
        public int ScoreId { get; set; }
        public Guid UserId { get; set; }
        public int GameId { get; set; }
        public int Score { get; set; }
        public DateTime AchievedAt { get; set; }

        public Users? user { get; set; }
        public Games? game { get; set; }
    }
}
