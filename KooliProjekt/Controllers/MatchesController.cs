using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly ApplicationDbContext _context;

        // Конструктор (без конфликта имен)
        public MatchesController(IMatchService matchService, ApplicationDbContext context)
        {
            _matchService = matchService;
            _context = context;
        }

        // Метод Index с пагинацией и поиском
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            MatchSearch? search = null) // Указание nullable для параметра
        {
            ViewBag.Tournaments = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                await _context.Tournaments.ToListAsync(),
                "Id",
                "TournamentName"
            );

            var model = new MatchesIndexModel
            {
                Matches = await _matchService.List(page, pageSize, search),
                Search = search ?? new MatchSearch()
            };

            return View(model);
        }

        // Метод Details (обработка nullable ID)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var match = await _matchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // Метод Create (GET)
        public IActionResult Create()
        {
            return View();
        }

        // Метод Create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstTeamId,SecondTeamId,MatchDate")] Match match)
        {
            if (!ModelState.IsValid) return View(match);

            await _matchService.Save(match);
            return RedirectToAction(nameof(Index));
        }

        // Метод Edit (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var match = await _matchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // Метод Edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstTeamId,SecondTeamId,MatchDate")] Match match)
        {
            if (id != match.Id) return NotFound();
            if (!ModelState.IsValid) return View(match);

            try
            {
                await _matchService.Edit(match);
            }
            catch
            {
                if (!await _matchService.Exists(match.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод Delete (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var match = await _matchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // Метод Delete (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _matchService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}