// KooliProjekt/Models/PagedResult.cs
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Models
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }
        public List<T> Items { get; internal set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}