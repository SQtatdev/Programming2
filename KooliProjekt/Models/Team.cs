using System.Collections.Generic;
using KooliProjekt.Models;
namespace KooliProjekt.Models
{
    public class Team
    {
        public int Id { get; set; }
        public required string TeamName { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();
        public List<Match> HomeMatches { get; set; } = new List<Match>();
        public List<Match> AwayMatches { get; set; } = new List<Match>();
    }
}