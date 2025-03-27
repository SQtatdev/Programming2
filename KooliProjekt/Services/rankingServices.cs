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
            var query = _context.Rankings.OrderBy(r => r.Id).AsNoTracking();
            var totalCount = await query.CountAsync();
            var rankings = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Ranking>
            {
                Items = rankings,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Ranking> GetById(int id)
        {
            return await _context.Rankings.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Save(Ranking ranking)
        {
            _context.Rankings.Add(ranking);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Ranking ranking)
        {
            _context.Rankings.Update(ranking);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking != null)
            {
                _context.Rankings.Remove(ranking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Rankings.AnyAsync(r => r.Id == id);
        }
    }
}