using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly Data.ApplicationDbContext _context;

        // Конструктор для внедрения зависимости ApplicationDbContext
        public TournamentService(Data.ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Получение турниров с пагинацией
        public async Task<PagedResult<Tournament>> List(int page, int pageSize)
        {
            var query = _context.Tournaments.OrderBy(t => t.TournamentStart).AsNoTracking(); // Не отслеживаем изменения для повышения производительности

            return await query.ToPagedResult(page, pageSize);
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