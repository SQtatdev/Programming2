using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        // Метод для отображения списка турниров с пагинацией
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            // Получаем данные с пагинацией
            var data = await _tournamentService.List(page, pageSize);
            return View(data);  // Передаем данные в представление
        }

        // Метод для отображения деталей турнира
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Получаем турнир по ID
            var tournament = await _tournamentService.GetById(id.Value);
            return tournament == null ? NotFound() : View(tournament);
        }

        // Метод для отображения формы создания турнира
        public IActionResult Create()
        {
            return View();
        }

        // Метод для обработки данных и создания турнира
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TournamentName,TournamentStart,TournamentEnd,TournamentPart,TournamentInfo")] Tournament tournament)
        {
            if (!ModelState.IsValid) return View(tournament);

            // Сохраняем турнир
            await _tournamentService.Save(tournament);
            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы редактирования турнира
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Получаем турнир по ID
            var tournament = await _tournamentService.GetById(id.Value);
            return tournament == null ? NotFound() : View(tournament);
        }

        // Метод для обработки редактирования турнира
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TournamentName,TournamentStart,TournamentEnd,TournamentPart,TournamentInfo")] Tournament tournament)
        {
            if (id != tournament.Id) return NotFound();
            if (!ModelState.IsValid) return View(tournament);

            try
            {
                // Редактируем турнир
                await _tournamentService.Edit(tournament);
            }
            catch
            {
                if (!await _tournamentService.Exists(tournament.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы удаления турнира
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Получаем турнир по ID
            var tournament = await _tournamentService.GetById(id.Value);
            return tournament == null ? NotFound() : View(tournament);
        }

        // Метод для удаления турнира
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Удаляем турнир
            await _tournamentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
