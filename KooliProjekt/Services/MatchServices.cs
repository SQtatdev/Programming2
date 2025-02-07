using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace KooliProjekt.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Метод для получения списка матчей с фильтрами
        public async Task<PagedResult<Match>> List(int page, int pageSize, MatchSearch search)
        {
            var query = _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrEmpty(search.TeamName))
            {
                query = query.Where(m => m.FirstTeam.TeamName.Contains(search.TeamName) || m.SecondTeam.TeamName.Contains(search.TeamName));
            }

            if (search.DateFrom.HasValue)
            {
                query = query.Where(m => m.GameStart >= search.DateFrom.Value);
            }

            if (search.DateTo.HasValue)
            {
                query = query.Where(m => m.GameStart <= search.DateTo.Value);
            }

            if (search.TournamentId.HasValue)
            {
                query = query.Where(m => m.TournamentId == search.TournamentId);
            }

            return await query.OrderBy(m => m.GameStart).GetPagedAsync(page, pageSize);
        }

        // Метод для получения матча по Id
        public async Task<Match> Get(int id)
        {
            var match = await _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);

            return match; // Если матч не найден, вернется null
        }

        // Метод для сохранения матча
        public async Task Save(Match match)
        {
            if (match.Id == 0) // Новый матч
            {
                _context.Matches.Add(match);
            }
            else // Обновление существующего матча
            {
                _context.Matches.Update(match);
            }

            await _context.SaveChangesAsync(); // Сохраняем изменения
        }

        // Метод для удаления матча
        public async Task Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id); // Ищем матч по id

            if (match != null)
            {
                _context.Matches.Remove(match); // Удаляем матч
                await _context.SaveChangesAsync(); // Сохраняем изменения
            }
        }
    }
}
