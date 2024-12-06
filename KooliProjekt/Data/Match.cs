namespace KooliProjekt.Data
{
    public class Match
    {
        public int Id { get; set; }

        public Tournament Tournament { get; set; }
        public int TournamentId { get; set; }
        
        public Team FirstTeam { get; set; }
        public int FirstTeamId { get; set; }

        public Team SecondTeam { get; set; }
        public int SecondTeamId { get; set; }
        public DateTime GameStart { get; set; }
        public int FirstTeamScore { get; set; }
        public int SecondTeamScore { get; set; }
        public string winner { get; set; }

        public IList<Prediction> Predictions { get; set; }

        public Match()
        { 
            Predictions = new List<Prediction>();
        }
    }
}
