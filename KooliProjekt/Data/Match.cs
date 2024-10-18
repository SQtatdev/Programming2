namespace KooliProjekt.Data
{
    public class Match
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int FirstTeamId { get; set; }
        public int SecondTeamId { get; set; }
        public DateTime GameStart { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }
        public string winner { get; set; }

    }
}
