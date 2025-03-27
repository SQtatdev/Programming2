namespace KooliProjekt.Models
{
    public class Ranking
    {
        public int Id { get; set; }
        public int TournamentID { get; set; }
        public int UserId { get; set; }
        public int TotalPoint { get; set; }
    }
}
