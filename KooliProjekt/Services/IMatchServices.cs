using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;

public interface IMatchService
{
    Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search);
    Task<IList<Match>> GetAll();
    Task<Match> GetById(int id);
    Task<Match> Create(Match match);
    Task Save(Match match);
    Task Edit(Match match);
    Task<bool> Update(Match match);
    Task Delete(int id);
    Task<bool> Exists(int id);
}