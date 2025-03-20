using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext _context;

        public PredictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получение предсказаний с пагинацией
        public async Task<PagedResult<Prediction>> List(int page, int pageSize)
        {
            var query = _context.Predictions.OrderBy(p => p.Id).AsNoTracking(); // Улучшение производительности, так как не отслеживаем изменения
            var totalCount = await query.CountAsync(); // Получаем общее количество предсказаний
            var predictions = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); // Получаем предсказания для текущей страницы

            return new PagedResult<Prediction>
            {
                Items = predictions,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        // Получение предсказания по ID
        public async Task<Prediction> GetById(int id)
        {
            return await _context.Predictions
                .AsNoTracking() // Не отслеживаем изменения
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Сохранение предсказания
        public async Task Save(Prediction prediction)
        {
            if (prediction == null)
                throw new ArgumentNullException(nameof(prediction), "Prediction cannot be null.");

            _context.Add(prediction);
            await _context.SaveChangesAsync();
        }

        // Редактирование предсказания
        public async Task Edit(Prediction prediction)
        {
            if (prediction == null)
                throw new ArgumentNullException(nameof(prediction), "Prediction cannot be null.");

            _context.Update(prediction);
            await _context.SaveChangesAsync();
        }

        // Удаление предсказания
        public async Task Delete(int id)
        {
            var prediction = await _context.Predictions.FindAsync(id);
            if (prediction == null)
                throw new KeyNotFoundException($"Prediction with ID {id} not found.");

            _context.Predictions.Remove(prediction);
            await _context.SaveChangesAsync();
        }

        // Проверка существования предсказания
        public async Task<bool> Exists(int id)
        {
            return await _context.Predictions.AnyAsync(p => p.Id == id);
        }
    }
}
