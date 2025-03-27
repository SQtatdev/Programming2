using KooliProjekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Наследуемся от IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Player> Players { get; set; } // Добавляем DbSet для Player

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Важно для настройки Identity

            // Настройка отношения Match -> FirstTeam
            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstTeam)
                .WithMany(t => t.HomeMatches) // Предполагаем, что Team имеет коллекцию HomeMatches
                .HasForeignKey(m => m.FirstTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка отношения Match -> SecondTeam
            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondTeam)
                .WithMany(t => t.AwayMatches) // Предполагаем, что Team имеет коллекцию AwayMatches
                .HasForeignKey(m => m.SecondTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка отношения Match -> Tournament
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches) // Предполагаем, что Tournament имеет коллекцию Matches
                .HasForeignKey(m => m.TournamentId);

            // Настройка отношения Team -> Players
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление игроков при удалении команды

            // Дополнительные настройки при необходимости...
        }
    }
}