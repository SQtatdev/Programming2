using KooliProjekt.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class Match
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } 
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public int TournamentId { get; set; }

        [Required]
        public string HomeTeamName { get; set; }
        [Required]
        public string AwayTeamName { get; set; }

        [Required]
        public Team FirstTeam { get; set; }
        [Required]
        public Team SecondTeam { get; set; }
        [Required]
        public Tournament Tournament { get; set; }

        public string Players { get; set; }

        public DateOnly GameStart { get; set; }
        public DateOnly GameEnd { get; set; }

        public int winner { get; set; }
    }
}