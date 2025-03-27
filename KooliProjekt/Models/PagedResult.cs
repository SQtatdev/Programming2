using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } 
        public int TotalCount { get; set; } 
        public int Page { get; set; } 
        public int PageSize { get; set; }
        public int CurrentPage { get; internal set; }
        public int RowCount { get; internal set; }
        public int PageCount { get; internal set; }
        public List<object> Results { get; internal set; }

        public async Task<PagedResult<Team>> List(int page, int pageSize)
        {
            var query = _context.Teams.OrderBy(t => t.Id).AsNoTracking();
            var totalCount = await query.CountAsync();
            var teams = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Team>
            {
                Items = teams,
                TotalCount = totalCount,
                Page = page, // Используйте "Page"
                PageSize = pageSize
            };
        }
    }
}