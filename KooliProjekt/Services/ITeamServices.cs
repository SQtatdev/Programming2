using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ITeamService
    {
        Task<PagedResult<Team>> List(int page, int pageSize);
        Task<Team> GetById(int id);
        Task<bool> Save(Team team);
        Task<bool> Update(Team team);
        Task<bool> Delete(int id);
        Task<bool> Edit(Team team);
        Task<bool> Exists(int id);
    }
}