using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get a paginated list of tournaments
        public async Task<PagedResult<Tournament>> List(int page, int pageSize)
        {
            return await _context.tournaments.GetPagedAsync(page, pageSize);
        }

        // Get a specific tournament by ID
        public async Task<Tournament> Get(int id)
        {
            return await _context.tournaments.FirstOrDefaultAsync(t => t.Id == id);
        }

        // Save a new or existing tournament
        public async Task Save(Tournament tournament)
        {
            if (tournament.Id == 0)
            {
                _context.tournaments.Add(tournament);
            }
            else
            {
                _context.tournaments.Update(tournament);
            }

            await _context.SaveChangesAsync();
        }

        // Delete a tournament by ID
        public async Task Delete(int id)
        {
            var tournament = await _context.tournaments.FindAsync(id);
            if (tournament != null)
            {
                _context.tournaments.Remove(tournament);
                await _context.SaveChangesAsync();
            }
        }
    }
}
