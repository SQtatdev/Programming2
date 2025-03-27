using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        // Метод для отображения списка матчей с пагинацией
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            // Получаем данные с пагинацией
            var data = await _matchService.List(page, pageSize);
            return View(data);  // Передаем данные в представление
        }

        // Метод для отображения деталей матча
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Получаем матч по ID
            var match = await _matchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // Метод для отображения формы создания матча
        public IActionResult Create()
        {
            return View();
        }

        // Метод для обработки данных и создания матча
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstTeamId,SecondTeamId,MatchDate")] Match match)
        {
            if (!ModelState.IsValid) return View(match);

            // Сохраняем матч
            await _matchService.Save(match);
            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы редактирования матча
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Получаем матч по ID
            var match = await _matchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // Метод для обработки редактирования матча
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstTeamId,SecondTeamId,MatchDate")] Match match)
        {
            if (id != match.Id) return NotFound();
            if (!ModelState.IsValid) return View(match);

            try
            {
                // Редактируем матч
                await _matchService.Edit(match);
            }
            catch
            {
                if (!await _matchService.Exists(match.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы удаления матча
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Получаем матч по ID
            var match = await _matchService.GetById(id.Value);
            return match == null ? NotFound() : View(match);
        }

        // Метод для удаления матча
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Удаляем матч
            await _matchService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
