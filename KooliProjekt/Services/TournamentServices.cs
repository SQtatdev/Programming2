using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получение турниров с пагинацией
        public async Task<PagedResult<Tournament>> List(int page, int pageSize)
        {
            var query = _context.Tournaments.OrderBy(t => t.TournamentStart).AsNoTracking(); // Не отслеживаем изменения для повышения производительности
            var totalCount = await query.CountAsync(); // Получаем общее количество турниров
            var tournaments = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); // Получаем турниры для текущей страницы

            return new PagedResult<Tournament>
            {
                Items = tournaments,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        // Получение турнира по ID
        public async Task<Tournament> GetById(int id)
        {
            return await _context.Tournaments
                .AsNoTracking() // Для повышения производительности
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Сохранение турнира
        public async Task Save(Tournament tournament)
        {
            if (tournament == null)
                throw new ArgumentNullException(nameof(tournament), "Tournament cannot be null.");

            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
        }

        // Редактирование турнира
        public async Task Edit(Tournament tournament)
        {
            if (tournament == null)
                throw new ArgumentNullException(nameof(tournament), "Tournament cannot be null.");

            _context.Tournaments.Update(tournament);
            await _context.SaveChangesAsync();
        }

        // Удаление турнира
        public async Task Delete(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
                throw new KeyNotFoundException($"Tournament with ID {id} not found.");

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
        }

        // Проверка существования турнира
        public async Task<bool> Exists(int id)
        {
            return await _context.Tournaments.AnyAsync(t => t.Id == id);
        }
    }
}
