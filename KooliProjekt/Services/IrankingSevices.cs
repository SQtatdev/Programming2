using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IRankingServices
    {
        Task<PagedResult<Ranking>> List(int page, int pageSize);
        Task<Ranking> GetById(int id);
        Task<bool> Save(Ranking ranking);
        Task<bool> Edit(Ranking ranking);
        Task<bool> Delete(int id);
        Task<bool> Exists(int id);
        Task UpdateAllRankings();
    }
}