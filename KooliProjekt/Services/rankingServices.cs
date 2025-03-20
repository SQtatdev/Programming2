using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class RankingService : IRankingService
    {
        private readonly ApplicationDbContext _context;

        public RankingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получение рейтингов с пагинацией
        public async Task<PagedResult<Ranking>> List(int page, int pageSize)
        {
            var query = _context.Rankings.OrderBy(r => r.Id).AsNoTracking(); // Не отслеживаем изменения для повышения производительности
            var totalCount = await query.CountAsync(); // Получаем общее количество рейтингов
            var rankings = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); // Получаем рейтинги для текущей страницы

            return new PagedResult<Ranking>
            {
                Items = rankings,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        // Получение рейтинга по ID
        public async Task<Ranking> GetById(int id)
        {
            return await _context.Rankings
                .AsNoTracking() // Для повышения производительности
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        // Сохранение рейтинга
        public async Task Save(Ranking ranking)
        {
            if (ranking == null)
                throw new ArgumentNullException(nameof(ranking), "Ranking cannot be null.");

            _context.Add(ranking);
            await _context.SaveChangesAsync();
        }

        // Редактирование рейтинга
        public async Task Edit(Ranking ranking)
        {
            if (ranking == null)
                throw new ArgumentNullException(nameof(ranking), "Ranking cannot be null.");

            _context.Update(ranking);
            await _context.SaveChangesAsync();
        }

        // Удаление рейтинга
        public async Task Delete(int id)
        {
            var ranking = await _context.Rankings.FindAsync(id);
            if (ranking == null)
                throw new KeyNotFoundException($"Ranking with ID {id} not found.");

            _context.Rankings.Remove(ranking);
            await _context.SaveChangesAsync();
        }

        // Проверка существования рейтинга
        public async Task<bool> Exists(int id)
        {
            return await _context.Rankings.AnyAsync(r => r.Id == id);
        }
    }
}
