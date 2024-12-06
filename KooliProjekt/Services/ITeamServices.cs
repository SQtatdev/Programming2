using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ITeamService
    {
        Task<PagedResult<Team>> List(int page, int pageSize);
        Task<Team> Get(int id);
        Task Save(Team team);
        Task Delete(int id);
    }
}
