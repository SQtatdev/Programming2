using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<Tournament>> List(int page, int pageSize)
        {
            return await _context.Tournaments
                .Include(t => t.Matches)
                .OrderBy(t => t.TournamentStart)
                .AsNoTracking()
                .ToPagedResult(page, pageSize);
        }

        public async Task<Tournament> GetById(int id)
        {
            return await _context.Tournaments
                .Include(t => t.Matches)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> Save(Tournament tournament)
        {
            try
            {
                if (tournament == null)
                    throw new ArgumentNullException(nameof(tournament));

                if (HasOverlappingTournaments(tournament.TournamentStart, tournament.TournamentEnd))
                    throw new InvalidOperationException("The tournament overlaps with existing tournaments");

                _context.Tournaments.Add(tournament);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Tournament tournament)
        {
            try
            {
                if (tournament == null)
                    throw new ArgumentNullException(nameof(tournament));

                if (HasOverlappingTournaments(tournament.TournamentStart, tournament.TournamentEnd, tournament.Id))
                    throw new InvalidOperationException("The tournament overlaps with existing tournaments");

                _context.Tournaments.Update(tournament);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var tournament = await _context.Tournaments.FindAsync(id);
                if (tournament == null)
                    return false;

                _context.Tournaments.Remove(tournament);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Tournaments.AnyAsync(t => t.Id == id);
        }

        public bool HasOverlappingTournaments(DateTime start, DateTime end, int? excludeId = null)
        {
            var query = _context.Tournaments.Where(t =>
                (t.TournamentStart <= end && t.TournamentEnd >= start));

            if (excludeId.HasValue)
            {
                query = query.Where(t => t.Id != excludeId.Value);
            }

            return query.Any();
        }
    }
}