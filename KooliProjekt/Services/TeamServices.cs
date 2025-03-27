using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KooliProjekt.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Team>> List(int page, int pageSize)
        {
            // Исправлено: _context.Teams (с заглавной буквы)
            var query = _context.Teams.OrderBy(t => t.Id).AsNoTracking();
            var totalCount = await query.CountAsync();
            var teams = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Team>
            {
                Items = teams,
                TotalCount = totalCount,
                Page = page, // Исправлено: Page вместо PageNumber (если используется PagedResult<T> из предыдущих примеров)
                PageSize = pageSize
            };
        }

        public async Task<Team> GetById(int id)
        {
            // Убрано AsNoTracking(), если планируется редактирование
            return await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Save(Team team)
        {
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Team team)
        {
            _context.Entry(team).State = EntityState.Modified; // Более явное обновление
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }
    }
}