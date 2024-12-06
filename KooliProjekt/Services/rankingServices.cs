using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class RankingService : IRankingService
    {
        private readonly ApplicationDbContext _context;

        public RankingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ranking>> List(int page, int pageSize)
        {
            return await _context.rankings.GetPagedAsync(page, pageSize);
        }

        public async Task<ranking> Get(int id)
        {
            return await _context.rankings.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Save(ranking ranking)
        {
            if (ranking.Id == 0)
            {
                _context.rankings.Add(ranking);
            }
            else
            {
                _context.rankings.Update(ranking);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ranking = await _context.rankings.FindAsync(id);
            if (ranking != null)
            {
                _context.rankings.Remove(ranking);
                await _context.SaveChangesAsync();
            }
        }
    }
}
