using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Models
{

    public class Ranking
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TournamentID { get; set; }
        public int TotalPoints { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}