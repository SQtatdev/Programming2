using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;
        private const int MaxPageSize = 100;

        public MatchService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search)
        {
            ValidatePaginationParameters(page, pageSize);

            var query = BuildSearchQuery(search ?? new MatchSearch());
            var totalCount = await query.CountAsync();

            var matches = await query
                .OrderBy(m => m.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Match>
            {
                Items = matches,
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<IList<Match>> GetAll()
        {
            return await BuildBaseQuery()
                .ToListAsync();
        }

        public async Task<Match> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid match ID", nameof(id));

            return await BuildBaseQuery()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Match> Create(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            ValidateMatch(match);

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task Save(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            ValidateMatch(match);

            if (match.Id == 0)
            {
                _context.Matches.Add(match);
            }
            else
            {
                _context.Matches.Update(match);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            ValidateMatch(match);

            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(Match match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            ValidateMatch(match);

            try
            {
                var existingMatch = await _context.Matches.FindAsync(match.Id);
                if (existingMatch == null)
                    return false;

                _context.Entry(existingMatch).CurrentValues.SetValues(match);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid match ID", nameof(id));

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                throw new KeyNotFoundException($"Match with ID {id} not found");

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            if (id <= 0)
                return false;

            return await _context.Matches
                .AnyAsync(m => m.Id == id);
        }

        private IQueryable<Match> BuildBaseQuery()
        {
            return _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .AsNoTracking();
        }

        private IQueryable<Match> BuildSearchQuery(MatchSearch search)
        {
            var query = BuildBaseQuery();

            if (!string.IsNullOrWhiteSpace(search.TeamName))
            {
                query = query.Where(m =>
                    m.FirstTeam.TeamName.Contains(search.TeamName) ||
                    m.SecondTeam.TeamName.Contains(search.TeamName));
            }

            if (search.StartDate.HasValue)
            {
                var startDate = DateOnly.FromDateTime(search.StartDate.Value);
                query = query.Where(m => m.Date >= startDate);
            }

            if (search.EndDate.HasValue)
            {
                var endDate = DateOnly.FromDateTime(search.EndDate.Value);
                query = query.Where(m => m.Date <= endDate);
            }

            if (search.TournamentId.HasValue)
            {
                query = query.Where(m => m.TournamentId == search.TournamentId);
            }

            return query;
        }

        private void ValidateMatch(Match match)
        {
            if (match.Date == default)
                throw new ArgumentException("Match date is required");

            if (match.FirstTeamId <= 0 || match.SecondTeamId <= 0)
                throw new ArgumentException("Team IDs must be positive");

            if (match.FirstTeamId == match.SecondTeamId)
                throw new ArgumentException("A team cannot play against itself");
        }

        private void ValidatePaginationParameters(int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentException("Page number must be at least 1", nameof(page));

            if (pageSize < 1 || pageSize > MaxPageSize)
                throw new ArgumentException($"Page size must be between 1 and {MaxPageSize}", nameof(pageSize));
        }
    }
}