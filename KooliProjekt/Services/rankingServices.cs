using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class RankingServices : IRankingServices
    {
        private readonly ApplicationDbContext _context;

        public RankingServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Ranking>> List(int page, int pageSize)
        {
            return await _context.Rankings
                .Include(r => r.TournamentID)
                .Include(r => r.User)
                .OrderBy(r => r.Id)
                .AsNoTracking()
                .ToPagedResult(page, pageSize);
        }

        public async Task<Ranking> GetById(int id)
        {
            return await _context.Rankings
                .Include(r => r.TournamentID)
                .Include(r => r.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public Ranking GetUserRanking(string userId)
        {
            return _context.Rankings
                .FirstOrDefault(r => r.UserId == userId);
        }

        public async Task<bool> Save(Ranking ranking)
        {
            try
            {
                _context.Rankings.Add(ranking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Ranking ranking)
        {
            try
            {
                var existing = await _context.Rankings.FindAsync(ranking.Id);
                if (existing == null) return false;

                _context.Entry(existing).CurrentValues.SetValues(ranking);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking == null) return false;

            _context.Rankings.Remove(ranking);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Rankings.AnyAsync(r => r.Id == id);
        }

        public async Task UpdateAllRankings()
        {
            // Логика обновления всех рейтингов
            // Например:
            var predictions = await _context.Predictions.ToListAsync();
            // Вычислить новые баллы и обновить рейтинги
            await _context.SaveChangesAsync();
        }

    }
}