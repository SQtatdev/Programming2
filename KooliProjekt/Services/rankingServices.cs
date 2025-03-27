using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class RankingServices : IRankingService
    {
        private readonly Data.ApplicationDbContext _context;

        public RankingServices(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Ranking>> List(int page, int pageSize)
        {
            var query = _context.rankings.OrderBy(r => r.Id).AsNoTracking();
            var totalCount = await query.CountAsync();
            var rankings = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Ranking>
            {
                Items = ranking,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        public async Task<Ranking> GetById(int id)
        {
            return await _context.rankings.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Save(Ranking ranking)
        {
            _context.rankings.Add(ranking);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Ranking ranking)
        {
            _context.rankings.Update(ranking);
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

        public async Task<bool> Exists(int id)
        {
            return await _context.rankings.AnyAsync(r => r.Id == id);
        }
    }
}