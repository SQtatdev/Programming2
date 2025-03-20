using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ITeamService
    {
        /// <summary>
        /// Получает список команд с пагинацией.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Количество элементов на странице.</param>
        /// <returns>Результат пагинированного списка команд.</returns>
        Task<PagedResult<Team>> List(int page, int pageSize);

        /// <summary>
        /// Получает команду по ID.
        /// </summary>
        /// <param name="id">ID команды.</param>
        /// <returns>Команда с указанным ID.</returns>
        Task<Team> GetById(int id);

        /// <summary>
        /// Сохраняет новую команду или обновляет существующую.
        /// </summary>
        /// <param name="team">Объект команды для сохранения.</param>
        Task Save(Team team);

        /// <summary>
        /// Редактирует существующую команду.
        /// </summary>
        /// <param name="team">Объект команды для редактирования.</param>
        Task Edit(Team team);

        /// <summary>
        /// Удаляет команду по ID.
        /// </summary>
        /// <param name="id">ID команды для удаления.</param>
        Task Delete(int id);

        /// <summary>
        /// Проверяет существование команды по ID.
        /// </summary>
        /// <param name="id">ID команды для проверки.</param>
        /// <returns>True, если команда существует, иначе False.</returns>
        Task<bool> Exists(int id);
    }
}
