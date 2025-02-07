using MathComapare.Context;
using MathComapare.Entities;
using MathComapare.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MathComapare.Repositories
{
    public class MathCompareRepositories : IMathCompareRepositories
    {
        private readonly CheckmathDBContext _context;
        public MathCompareRepositories(CheckmathDBContext context) => _context = context;


        public async Task<List<Users>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<Scores> GetScore(Users data)
        {
            return await _context.Scores.FindAsync(data.Name);      
        }

        public async Task<Users> GetUserInfoAsync(Guid data)
        {
            return await _context.Users.FindAsync(data);
        }

        public async Task RegisterUserAsync(Users user_data)
        {
            var user_name = user_data.Name;
            var email = user_data.Email;

            var user_info = new Users
            {
                UserId = Guid.NewGuid(),
                Name = user_name,
                Email = email,
                GoogleId = user_data.GoogleId,
                CreatedAt = user_data.CreatedAt,
                LastLogin = DateTime.UtcNow
            };

            _context.Users.Add(user_info);
            await _context.SaveChangesAsync();
        }

        public async Task SubmmitScores(Scores data)
        {
            var user_score = new Scores
            {
                AchievedAt = DateTime.UtcNow,
                GameId = data.GameId,
                ScoreId = data.ScoreId,
                UserId = data.UserId,
                Score = data.Score,
            };

            _context.Scores.Add(user_score);
            await _context.SaveChangesAsync();
        }
    }
}
