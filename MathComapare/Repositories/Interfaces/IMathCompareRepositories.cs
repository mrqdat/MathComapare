using MathComapare.Entities;

namespace MathComapare.Repositories.Interfaces
{
    public interface IMathCompareRepositories
    {
        Task RegisterUserAsync(Users data);
        Task<Users> GetUserInfoAsync(Guid data);
        Task<List<Users>> GetAllUser();
        Task SubmmitScores(Scores data);
        Task<Scores> GetScore(Users data);

    }
}
