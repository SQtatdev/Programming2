// KooliProjekt/Models/PagedResult.cs
using System.Collections.Generic;

namespace KooliProjekt.Models
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }
        public List<Team> Items { get; internal set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}