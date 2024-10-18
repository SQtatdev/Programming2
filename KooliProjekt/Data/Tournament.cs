namespace KooliProjekt.Data
{
    public class Tournament
    {
        public int Id { get; set; }
        public string TournmentName { get; set; }
        public DateTime TournamentStart { get; set; }
        public DateTime TournamentEnd { get; set; }
        public int TournamentPart {  get; set; }
        public string TournamentInfo { get; set; }

    }
}
