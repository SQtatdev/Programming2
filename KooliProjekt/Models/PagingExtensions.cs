using System.Collections.Generic;

namespace KooliProjekt.Models
{
    public static class PagingExtensions
    {
        public static PagedResult<T> ToPagedResult<T>(
            this IEnumerable<T> items,
            int page,
            int pageSize,
            int totalCount)
        {
            return new PagedResult<T>
            {
                Items = new List<T>(items),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}