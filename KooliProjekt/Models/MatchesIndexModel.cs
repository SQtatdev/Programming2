using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Data;
using KooliProjekt.Search;




namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class MatchesIndexModel
    {
        public MatchSearch Search { get; set; }
        public PagedResult<Match> Matches { get; set; }
    }
}
