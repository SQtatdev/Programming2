using KooliProjekt.Data;
using KooliProjekt.Models;

namespace KooliProjekt.Data
{
    public class Match
    {
        public int Id { get; set; }
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public DateTime GameStart { get; set; }
        public int TournamentId { get; set; }

        // Навигационные свойства
        public required Team FirstTeam { get; set; }
        public required Team SecondTeam { get; set; }
        public required Tournament Tournament { get; set; }
    }
}