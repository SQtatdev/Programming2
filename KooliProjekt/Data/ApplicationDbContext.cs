using KooliProjekt.Models;
using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Ranking> Rankings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstTeam)
                .WithMany()
                .HasForeignKey(m => m.FirstTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondTeam)
                .WithMany()
                .HasForeignKey(m => m.SecondTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany()
                .HasForeignKey(m => m.TournamentId);
        }
    }
}