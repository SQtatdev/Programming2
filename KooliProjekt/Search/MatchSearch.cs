namespace KooliProjekt.Search
{
    public class MatchSearch
    {
        public string TeamName { get; set; }  // Фильтрация по названию команды
        public DateTime? DateFrom { get; set; }  // Фильтрация по дате начала матча (от)
        public DateTime? DateTo { get; set; }  // Фильтрация по дате начала матча (до)
        public int? TournamentId { get; set; }  // Фильтрация по идентификатору турнира
    }
}
