using KooliProjekt.Models;

namespace KooliProjekt.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; } // Основное свойство даты
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public int TournamentId { get; set; }

        public Team FirstTeam { get; set; }
        public Team SecondTeam { get; set; }
        public Tournament Tournament { get; set; }

        public DateOnly GameStart { get; set; }
        public DateOnly GameEnd { get; set; }

        public int winner { get; set; }
    }
}