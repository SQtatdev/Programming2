using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class MatchesIndexModel
    {
        public MatchSearch Search { get; set; } = new();
        public PagedResult<Match> Matches { get; set; } = new();

    }
}