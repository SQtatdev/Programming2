using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ITournamentService
    {
        Task<PagedResult<Tournament>> List(int page, int pageSize);
        Task<Tournament> Get(int id);
        Task Save(Tournament tournament);
        Task Delete(int id);
    }
}
