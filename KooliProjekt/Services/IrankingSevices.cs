using KooliProjekt.Models;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IRankingService
    {
        Task<PagedResult<Ranking>> List(int page, int pageSize);
        Task<Ranking> GetById(int id);
        Task Save(Ranking ranking);
        Task Edit(Ranking ranking);
        Task Delete(int id);
        Task<bool> Exists(int id);
    }
}