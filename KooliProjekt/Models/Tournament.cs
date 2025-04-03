using KooliProjekt.Data;
using KooliProjekt.Models;

namespace KooliProjekt.Models
{
    public class Tournament
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required DateTime TournamentStart { get; set; }
        public required DateTime TournamentEnd { get; set; }
        public required int TournamentPart { get; set; }
        public required string TournamentInfo { get; set; }


        public required List<Match> Matches
        {
            get; set;
        }
        public required IList<Ranking> rankings { get; set; }

        public Tournament()
        {
            Matches = new List<Match>();
            rankings = new List<Ranking>();
        }


    }
}
