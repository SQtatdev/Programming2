using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Prediction> Predictions { get; set; }
    }
    public class Prediction
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int UserId { get; set; }
        public int PredictedScoreFirstTeam { get; set; }
        public int PredictedScoreSecondTeam { get; set; }
        public int Punktid { get; set; }
    }
}