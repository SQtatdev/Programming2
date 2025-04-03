using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class MatchesIndexModel
    {
        public MatchSearch Search { get; set; } = new();
        public PagedResult<Match> Matches { get; set; } = new();

    }
}