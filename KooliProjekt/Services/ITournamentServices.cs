using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ITournamentService
    {
        // Получение турниров с пагинацией
        Task<PagedResult<Tournament>> List(int page, int pageSize);

        // Получение турнира по ID
        Task<Tournament> GetById(int id);

        // Сохранение турнира
        Task Save(Tournament tournament);

        // Редактирование турнира
        Task Edit(Tournament tournament);

        // Удаление турнира
        Task Delete(int id);

        // Проверка существования турнира
        Task<bool> Exists(int id);
        void HasOverlappingTournaments(DateTime TournamentStart, DateTime TournamentEnd);
    }
}