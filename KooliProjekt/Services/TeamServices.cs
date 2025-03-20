using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        // Получение команд с пагинацией
        public async Task<PagedResult<Team>> List(int page, int pageSize)
        {
            var query = _context.Teams.OrderBy(t => t.TeamName).AsNoTracking(); // Не отслеживаем изменения для повышения производительности
            var totalCount = await query.CountAsync(); // Получаем общее количество команд
            var teams = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); // Получаем команды для текущей страницы

            return new PagedResult<Team>
            {
                Items = teams,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        // Получение команды по ID
        public async Task<Team> GetById(int id)
        {
            return await _context.Teams
                .AsNoTracking() // Для повышения производительности
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Сохранение команды
        public async Task Save(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "Team cannot be null.");

            _context.Add(team);
            await _context.SaveChangesAsync();
        }

        // Редактирование команды
        public async Task Edit(Team team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team), "Team cannot be null.");

            _context.Update(team);
            await _context.SaveChangesAsync();
        }

        // Удаление команды
        public async Task Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                throw new KeyNotFoundException($"Team with ID {id} not found.");

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
        }

        // Проверка существования команды
        public async Task<bool> Exists(int id)
        {
            return await _context.Teams.AnyAsync(t => t.Id == id);
        }
    }
}
