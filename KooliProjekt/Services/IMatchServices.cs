using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;

public interface IMatchService
{
    // Версия с фильтром
    Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search);

    Task<Match> GetById(int id);
    Task Save(Match match);
    Task Edit(Match match);
    Task Delete(int id);
    Task<bool> Exists(int id);
}