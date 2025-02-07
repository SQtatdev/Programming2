using KooliProjekt.Data;
using KooliProjekt.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface IMatchService
    {
        Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search);
        Task<Match> Get(int id);
        Task Save(Match match);
        Task Delete(int id);
    }
}
