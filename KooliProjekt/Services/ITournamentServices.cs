using KooliProjekt.Models;
using System;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public interface ITournamentService
    {
        Task<PagedResult<Tournament>> List(int page, int pageSize);
        Task<Tournament> GetById(int id);
        Task<bool> Save(Tournament tournament);
        Task<bool> Edit(Tournament tournament);
        Task<bool> Delete(int id);
        Task<bool> Exists(int id);
        bool HasOverlappingTournaments(DateTime start, DateTime end, int? excludeId = null);
    }
}