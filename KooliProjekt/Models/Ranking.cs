using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Models
{

    public class Ranking
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int TournamentID { get; set; }
        public int TotalPoints { get; set; }

        [ForeignKey("UserId")]
        public virtual required ApplicationUser User { get; set; }
    }
}