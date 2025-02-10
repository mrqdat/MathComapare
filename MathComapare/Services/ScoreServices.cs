using MathComapare.Entities;
using StackExchange.Redis;

namespace MathComapare.Services
{
    public class ScoreServices
    {
        private readonly IConnectionMultiplexer _redis;

        public ScoreServices(IConnectionMultiplexer redis) => _redis = redis;

        public async Task CacheScoreAsync (int gameId, int score)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync($"Game:{gameId}:Score",score);
        }

        public async Task<int> getScoreAsync(int gameId)
        {
            var db = _redis.GetDatabase();
            var score = await db.StringGetAsync($"Game:{gameId}:Score");
            return score.HasValue ? (int)score : 0;
        }
    }
}
