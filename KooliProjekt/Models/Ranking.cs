using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]

    public class Ranking
    {
        public int Id { get; set; }
        [Required]
        public int  UserId { get; set; }
        public int TournamentID { get; set; }
        public int TotalPoints { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}