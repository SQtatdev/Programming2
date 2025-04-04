﻿using KooliProjekt.Data;
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
        private readonly Data.ApplicationDbContext _context;

        public TeamService(Data.ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PagedResult<Team>> List(int page, int pageSize)
        {
            ValidatePaginationParameters(page, pageSize);

            var query = _context.Teams.OrderBy(t => t.Id).AsNoTracking();

            return await query.ToPagedResult(page, pageSize);
        }

        public async Task<Team> GetById(int id)
        {
            return await _context.Teams
                .FirstOrDefaultAsync(t => t.Id == id)
                ?? throw new KeyNotFoundException($"Team with id {id} not found");
        }

        public async Task Save(Team team)
        {
            ValidateTeam(team);

            _context.Teams.Add(team);
            await SaveChangesWithExceptionHandling();
        }

        public async Task Update(Team team)
        {
            ValidateTeam(team);

            var existingTeam = await GetExistingTeam(team.Id);
            _context.Entry(existingTeam).CurrentValues.SetValues(team);

            await SaveChangesWithExceptionHandling();
        }

        public async Task Delete(int id)
        {
            var team = await GetExistingTeam(id);

            _context.Teams.Remove(team);
            await SaveChangesWithExceptionHandling();
        }

        public async Task Edit(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }

        #region Helper Methods

        private async Task<List<Team>> GetPagedItems(IQueryable<Team> query, int page, int pageSize)
        {
            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

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

        private async Task SaveChangesWithExceptionHandling()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Database operation failed", ex);
            }
        }

        #endregion
    }
}