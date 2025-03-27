using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search; // Добавьте это пространство имен
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IMatchService
    {
        Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search);
        Task<Match> GetById(int id);
        Task Save(Match match);
        Task Edit(Match match);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<string?> List(int page, int pageSize);
    }
}