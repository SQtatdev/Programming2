using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IMatchService
    {
        Task<PagedResult<Match>> List(int page, int pageSize);
        Task<Match> Get(int id);
        Task Save(Match match);
        Task Delete(int id);
    }
}
