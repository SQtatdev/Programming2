using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IRankingService
    {
        Task<PagedResult<ranking>> List(int page, int pageSize);
        Task<ranking> Get(int id);
        Task Save(ranking ranking);
        Task Delete(int id);
    }
}
