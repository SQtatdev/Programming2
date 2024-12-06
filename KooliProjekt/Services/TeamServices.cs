using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

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
            return await _context.teams.GetPagedAsync(page, pageSize);
        }

        public async Task<Team> Get(int id)
        {
            return await _context.teams.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task Save(Team team)
        {
            if (team.Id == 0)
            {
                _context.teams.Add(team);
            }
            else
            {
                _context.teams.Update(team);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var team = await _context.teams.FindAsync(id);
            if (team != null)
            {
                _context.teams.Remove(team);
                await _context.SaveChangesAsync();
            }
        }
    }
}
