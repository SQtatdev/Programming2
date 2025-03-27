using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class MatchService : IMatchService
    {
        private readonly Data.ApplicationDbContext _context;

        public MatchService(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search)
        {
            var query = _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .AsNoTracking();

            // Фильтрация
            if (!string.IsNullOrEmpty(search.TeamName))
            {
                query = query.Where(m =>
                    m.FirstTeam.TeamName.Contains(search.TeamName) ||
                    m.SecondTeam.TeamName.Contains(search.TeamName));
            }

            if (search.DateFrom.HasValue)
            {
                query = query.Where(m => m.GameStart >= search.DateFrom);
            }

            if (search.DateTo.HasValue)
            {
                query = query.Where(m => m.GameStart <= search.DateTo);
            }

            if (search.TournamentId.HasValue)
            {
                query = query.Where(m => m.TournamentId == search.TournamentId);
            }



            // Пагинация
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(m => m.GameStart)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Match>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<Match>> List(int page, int pageSize)
        {
            var query = _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(m => m.GameStart)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Match>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<Match> GetById(int id)
        {
            return await _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Match match)
        {
            _context.Matches.Update(match);
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

        public async Task<bool> Exists(int id)
        {
            return await _context.Matches.AnyAsync(m => m.Id == id);
        }

        //public Task List(int page, int pageSize, MatchSearch search)
        //{
        //    throw new NotImplementedException();
        //}
    }
}