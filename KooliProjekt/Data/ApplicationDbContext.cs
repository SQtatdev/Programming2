using KooliProjekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Match - Team relationships
            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.FirstTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.SecondTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Match - Tournament relationship
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId);

            // Team - Players relationship
            modelBuilder.Entity<Team>()
                .HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ranking - User relationship (FIXED)
            modelBuilder.Entity<Ranking>(entity =>
            {
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(r => r.UserId);
            });
        }
    }
}