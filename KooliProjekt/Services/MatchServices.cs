using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Match>> List(int page, int pageSize)
        {
            return await _context.Matches.GetPagedAsync(page, pageSize);
        }

        public async Task<Match> Get(int id)
        {
            return await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Match match)
        {
            if (match.Id == 0)
            {
                _context.Add(match);
            }
            else
            {
                _context.Update(match);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }
    }
}
