namespace KooliProjekt.Models
{
    public class MatchesIndexModel
    {
        public MatchSearch Search { get; set; }  // Параметры поиска
        public PagedResult<Match> Matches { get; set; }  // Список матчей (пагинированные данные)
    }
}
