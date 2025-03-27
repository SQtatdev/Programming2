using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ITeamService
    {
        Task<PagedResult<Team>> List(int page, int pageSize);
        Task<Team> GetById(int id);
        Task Save(Team team);
        Task Edit(Team team);
        Task Delete(int id);
        Task<bool> Exists(int id);
    }
}