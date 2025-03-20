using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для получения списка матчей с пагинацией
        public async Task<PagedResult<Match>> List(int page, int pageSize)
        {
            var query = _context.Matches
                .OrderBy(m => m.MatchDate)
                .AsNoTracking(); // Не отслеживаем изменения для улучшения производительности

            var totalCount = await query.CountAsync();

            var matches = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Match>
            {
                Items = matches,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        // Метод для получения матча по ID
        public async Task<Match> GetById(int id)
        {
            return await _context.Matches
                .AsNoTracking() // Не отслеживаем изменения
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        // Метод для сохранения нового матча
        public async Task Save(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        // Метод для редактирования существующего матча
        public async Task Edit(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        // Метод для удаления матча
        public async Task Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                throw new KeyNotFoundException($"Match with ID {id} not found.");

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }

        // Метод для проверки существования матча
        public async Task<bool> Exists(int id)
        {
            return await _context.Matches.AnyAsync(m => m.Id == id);
        }
    }
}
