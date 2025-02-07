using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using System.Linq;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index(int page, MatchSearch search)
        {
            var query = _context.Matches
                .Include(m => m.FirstTeam)
                .Include(m => m.SecondTeam)
                .Include(m => m.Tournament)
                .AsQueryable();

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

            var model = new MatchesIndexModel
            {
                Search = search,
                Matches = await query.OrderBy(m => m.GameStart).GetPagedAsync(page, 5)
            };

            ViewData["Tournaments"] = new SelectList(_context.tournaments, "Id", "Name");

            return View(model);
        }
    }
}
