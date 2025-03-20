using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Получает список турниров с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество турниров на странице.</param>
        /// <returns>Пагинированный список турниров.</returns>
        public async Task<PagedResult<Tournament>> List(int page, int pageSize)
        {
            // Получаем общее количество турниров
            var totalCount = await _context.Tournaments.CountAsync();

            // Получаем турниры для указанной страницы
            var tournaments = await _context.Tournaments
                .Skip((page - 1) * pageSize)  // Пропускаем элементы в зависимости от страницы
                .Take(pageSize)  // Берем нужное количество турниров на страницу
                .ToListAsync();

            // Возвращаем пагинированный результат
            return new PagedResult<Tournament>
            {
                Items = tournaments,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Получает турнир по его ID.
        /// </summary>
        /// <param name="id">ID турнира.</param>
        /// <returns>Турнир с указанным ID.</returns>
        public async Task<Tournament> Get(int id)
        {
            // Получаем турнир по ID
            return await _context.Tournaments
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <summary>
        /// Сохраняет новый турнир или обновляет существующий.
        /// </summary>
        /// <param name="tournament">Турнир для сохранения или обновления.</param>
        public async Task Save(Tournament tournament)
        {
            try
            {
                if (tournament.Id == 0)
                {
                    // Новый турнир: добавляем в базу данных
                    _context.Tournaments.Add(tournament);
                }
                else
                {
                    // Существующий турнир: обновляем в базе данных
                    _context.Tournaments.Update(tournament);
                }

                await _context.SaveChangesAsync();  // Сохраняем изменения в базе данных
            }
            catch (Exception ex)
            {
                // Логирование ошибки или выброс исключения по мере необходимости
                throw new InvalidOperationException("Ошибка при сохранении турнира.", ex);
            }
        }

        /// <summary>
        /// Удаляет турнир по ID.
        /// </summary>
        /// <param name="id">ID турнира, который нужно удалить.</param>
        public async Task Delete(int id)
        {
            try
            {
                // Ищем турнир по ID
                var tournament = await _context.Tournaments.FindAsync(id);

                if (tournament != null)
                {
                    // Удаляем турнир из контекста
                    _context.Tournaments.Remove(tournament);
                    await _context.SaveChangesAsync();  // Сохраняем изменения в базе данных
                }
                else
                {
                    throw new KeyNotFoundException($"Турнир с ID {id} не найден.");
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки или выброс исключения по мере необходимости
                throw new InvalidOperationException("Ошибка при удалении турнира.", ex);
            }
        }
    }
}
