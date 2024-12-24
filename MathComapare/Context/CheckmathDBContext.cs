using MathComapare.Entities;
using Microsoft.EntityFrameworkCore;

namespace MathComapare.Context
{
    public class CheckmathDBContext : DbContext
    {
        public CheckmathDBContext(DbContextOptions<CheckmathDBContext> options) : base(options) { }

        public DbSet<Games> Games => Set<Games>();
        public DbSet<Users> Users => Set<Users>();
        public DbSet<Scores> Scores => Set<Scores>();

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source = checkmath.db");
        //}

        
    }
}
