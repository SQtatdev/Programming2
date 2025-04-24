using KooliProjekt.Data;
using KooliProjekt.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class Tournament
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime TournamentStart { get; set; }
        [Required]
        public DateTime TournamentEnd { get; set; }
        [Required]
        public int TournamentPart { get; set; }
        [Required]
        public string TournamentInfo { get; set; }


        public List<Match> Matches
        {
            get; set;
        }
        public IList<Ranking> rankings { get; set; }

        public Tournament()
        {
            Matches = new List<Match>();
            rankings = new List<Ranking>();
        }

    }
}
