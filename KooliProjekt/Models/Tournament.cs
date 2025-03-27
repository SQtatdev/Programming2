using KooliProjekt.Data;
using KooliProjekt.Models;

namespace KooliProjekt.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TournamentStart { get; set; }
        public DateTime TournamentEnd { get; set; }
        public int TournamentPart { get; set; }
        public string TournamentInfo { get; set; }


        public IList<Match> Matches { get; set; }
        public IList<Ranking> rankings { get; set; }

        public Tournament()
        {
            Matches = new List<Match>();
            rankings = new List<Ranking>();
        }


    }
}
