using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<Team>> List(int page, int pageSize)
        {
            ValidatePaginationParameters(page, pageSize);

            return await _context.Teams
                .Include(t => t.HomeMatches)
                .Include(t => t.AwayMatches)
                .OrderBy(t => t.Id)
                .AsNoTracking()
                .ToPagedResult(page, pageSize);
        }

        public async Task<Team> GetById(int id)
        {
            return await _context.Teams
                .Include(t => t.HomeMatches)
                .Include(t => t.AwayMatches)
                .FirstOrDefaultAsync(t => t.Id == id)
                ?? throw new KeyNotFoundException($"Team with id {id} not found");
        }

        public async Task<bool> Save(Team team)
        {
            ValidateTeam(team);

            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(Team team)
        {
            ValidateTeam(team);

            try
            {
                var existingTeam = await GetExistingTeam(team.Id);
                _context.Entry(existingTeam).CurrentValues.SetValues(team);
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
                var team = await GetExistingTeam(id);
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Edit(Team team)
        {
            try
            {
                _context.Teams.Update(team);
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
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }

        #region Helper Methods

        private void ValidatePaginationParameters(int page, int pageSize)
        {
            if (page < 1)
                throw new ArgumentException("Page number must be at least 1", nameof(page));

            if (pageSize < 1 || pageSize > 100)
                throw new ArgumentException("Page size must be between 1 and 100", nameof(pageSize));
        }

        private void ValidateTeam(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));
        }

        private async Task<Team> GetExistingTeam(int id)
        {
            return await _context.Teams.FindAsync(id)
                ?? throw new KeyNotFoundException($"Team with id {id} not found");
        }

        #endregion
    }
}