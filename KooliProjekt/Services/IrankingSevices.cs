using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IRankingService
    {
        /// <summary>
        /// Получает список рейтингов с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <returns>Результат пагинированного списка рейтингов.</returns>
        Task<PagedResult<Ranking>> List(int page, int pageSize);

        /// <summary>
        /// Получает рейтинг по ID.
        /// </summary>
        /// <param name="id">ID рейтинга.</param>
        /// <returns>Рейтинг с указанным ID.</returns>
        Task<Ranking> GetById(int id);

        /// <summary>
        /// Сохраняет новый рейтинг или обновляет существующий.
        /// </summary>
        /// <param name="ranking">Объект рейтинга для сохранения.</param>
        Task Save(Ranking ranking);

        /// <summary>
        /// Редактирует существующий рейтинг.
        /// </summary>
        /// <param name="ranking">Объект рейтинга для редактирования.</param>
        Task Edit(Ranking ranking);

        /// <summary>
        /// Удаляет рейтинг по ID.
        /// </summary>
        /// <param name="id">ID рейтинга для удаления.</param>
        Task Delete(int id);

        /// <summary>
        /// Проверяет существование рейтинга по ID.
        /// </summary>
        /// <param name="id">ID рейтинга для проверки.</param>
        /// <returns>True, если рейтинг существует, иначе False.</returns>
        Task<bool> Exists(int id);
    }
}
