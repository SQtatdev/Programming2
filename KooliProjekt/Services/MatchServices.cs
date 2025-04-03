using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class MatchService : IMatchService
    {
        private readonly Data.ApplicationDbContext _context; // Убрал лишний namespace Data

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

            // Исправлено: используем search.StartDate и search.EndDate
            if (search.StartDate.HasValue)
            {
                DateOnly startDateOnly = DateOnly.FromDateTime(search.StartDate.Value);
                query = query.Where(m => m.Date >= startDateOnly);
            }

            if (search.EndDate.HasValue)
            {
                DateOnly endDateOnly = DateOnly.FromDateTime(search.EndDate.Value);
                query = query.Where(m => m.Date <= endDateOnly);
            }

            if (search.TournamentId.HasValue)
            {
                query = query.Where(m => m.TournamentId == search.TournamentId);
            }

            // Пагинация
            var items = await query
                .OrderBy(m => m.Date) // Исправлено: было m.startDate
                .ToPagedResult(page, pageSize);

            return items;
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
                .OrderBy(m => m.Date) // Исправлено: было m.GameStart                
                .ToPagedResult(page, pageSize);
            return items;
        }

        public async Task<Match> GetById(int id)
        {
            return await _context.Matches // Исправлено: было _contexts
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
            // Более безопасный подход для редактирования
            var existing = await _context.Matches.FindAsync(match.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(match);
                await _context.SaveChangesAsync();
            }
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
    }
}