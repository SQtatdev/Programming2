using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchService _MatchService;

        // Конструктор с внедрением зависимости ITeamService
        public MatchesController(IMatchService MatchServices)
        {
            _MatchService = MatchServices;
        }

        // GET: Teams
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            // Create a default MatchSearch object (modify if needed)
            var search = new MatchSearch();

            // Pass all three required parameters
            var result = await _MatchService.List(page, pageSize, search);

            return View(result);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var matchid = await _MatchService.GetById(id.Value);
            return matchid == null ? NotFound() : View(matchid);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Match matchid)
        {
            if (!ModelState.IsValid) return View(matchid);

            await _MatchService.Save(matchid);
            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var team = await _MatchService.GetById(id.Value);
            return team == null ? NotFound() : View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Match match)
        {
            if (id != match.Id) return NotFound();
            if (!ModelState.IsValid) return View(match);

            try
            {
                await _MatchService.Edit(match);
            }
            catch
            {
                if (!await _MatchService.Exists(match.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var match = await _MatchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _MatchService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}