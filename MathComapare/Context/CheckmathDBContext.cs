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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserId)
                .HasName("PrimaryKey_userId");

            modelBuilder.Entity<Users>()
                .HasMany(g => g.Games)
                .WithOne(u => u.User)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Users>()
                .HasMany(g => g.Scores)
                .WithOne(u => u.user)
                .HasForeignKey(s => s.UserId);
            /////

            modelBuilder.Entity<Games>()
                .HasKey(s => s.GameId)
                .HasName("PrimaryKey_gameId");
            modelBuilder.Entity<Games>()
               .HasMany<Games>()
               .WithOne()
               .HasForeignKey(g => g.GameId);
            //////   

            modelBuilder.Entity<Scores>()
                .HasKey(s => s.ScoreId)
                .HasName("PrimaryKey_scoreId");

            modelBuilder.Entity<Scores>()
                .HasOne(g => g.game)
                .WithMany()
                .HasForeignKey(g => g.GameId);   

            //optionsBuilder.UseSqlite("Data Source = checkmath.db");
        }
    }
}
