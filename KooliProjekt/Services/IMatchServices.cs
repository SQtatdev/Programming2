using KooliProjekt.Models;
using KooliProjekt.Search;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IMatchService
    {
        /// <summary>
        /// Получает список матчей с поддержкой пагинации и фильтрации.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <param name="search">Объект фильтрации для матчей.</param>
        /// <returns>Результат пагинированного списка матчей.</returns>
        Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search);

        /// <summary>
        /// Получает матч по ID.
        /// </summary>
        /// <param name="id">ID матча.</param>
        /// <returns>Матч по указанному ID.</returns>
        Task<Match> Get(int id);

        /// <summary>
        /// Сохраняет новый матч или обновляет существующий.
        /// </summary>
        /// <param name="match">Объект матча для сохранения.</param>
        Task Save(Match match);

        /// <summary>
        /// Удаляет матч по ID.
        /// </summary>
        /// <param name="id">ID матча для удаления.</param>
        Task Delete(int id);
    }
}
