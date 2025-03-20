using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IPredictionService
    {
        /// <summary>
        /// Получает список предсказаний с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <returns>Результат пагинированного списка предсказаний.</returns>
        Task<PagedResult<Prediction>> List(int page, int pageSize);

        /// <summary>
        /// Получает предсказание по ID.
        /// </summary>
        /// <param name="id">ID предсказания.</param>
        /// <returns>Предсказание с указанным ID.</returns>
        Task<Prediction> GetById(int id);

        /// <summary>
        /// Сохраняет новое предсказание или обновляет существующее.
        /// </summary>
        /// <param name="prediction">Объект предсказания для сохранения.</param>
        Task Save(Prediction prediction);

        /// <summary>
        /// Редактирует существующее предсказание.
        /// </summary>
        /// <param name="prediction">Объект предсказания для редактирования.</param>
        Task Edit(Prediction prediction);

        /// <summary>
        /// Удаляет предсказание по ID.
        /// </summary>
        /// <param name="id">ID предсказания для удаления.</param>
        Task Delete(int id);

        /// <summary>
        /// Проверяет существование предсказания по ID.
        /// </summary>
        /// <param name="id">ID предсказания для проверки.</param>
        /// <returns>True, если предсказание существует, иначе False.</returns>
        Task<bool> Exists(int id);
    }
}
