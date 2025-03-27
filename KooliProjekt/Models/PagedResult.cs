// KooliProjekt/Models/PagedResult.cs
using System.Collections.Generic;

namespace KooliProjekt.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount => (int)System.Math.Ceiling((double)TotalCount / PageSize);
    }
}