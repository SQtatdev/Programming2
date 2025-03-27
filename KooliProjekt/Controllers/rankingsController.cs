using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class RankingsController : Controller
    {
        private readonly IRankingService _rankingService;

        public RankingsController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        // Метод для отображения списка рейтингов с пагинацией
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            // Получаем данные с пагинацией
            var data = await _rankingService.List(page, pageSize);
            return View(data);  // Передаем данные в представление
        }

        // Метод для отображения деталей рейтинга
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Получаем рейтинг по ID
            var ranking = await _rankingService.GetById(id.Value);
            return ranking == null ? NotFound() : View(ranking);
        }

        // Метод для отображения формы создания рейтинга
        public IActionResult Create()
        {
            return View();
        }

        // Метод для обработки данных и создания рейтинга
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TournamentId,UserId,TotalPoint")] Ranking ranking)
        {
            if (!ModelState.IsValid) return View(ranking);

            // Сохраняем рейтинг
            await _rankingService.Save(ranking);
            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы редактирования рейтинга
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Получаем рейтинг по ID
            var ranking = await _rankingService.GetById(id.Value);
            return ranking == null ? NotFound() : View(ranking);
        }

        // Метод для обработки редактирования рейтинга
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TournamentId,UserId,TotalPoint")] Ranking ranking)
        {
            if (id != ranking.Id) return NotFound();
            if (!ModelState.IsValid) return View(ranking);

            try
            {
                // Редактируем рейтинг
                await _rankingService.Edit(ranking);
            }
            catch
            {
                if (!await _rankingService.Exists(ranking.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы удаления рейтинга
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Получаем рейтинг по ID
            var ranking = await _rankingService.GetById(id.Value);
            return ranking == null ? NotFound() : View(ranking);
        }

        // Метод для удаления рейтинга
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Удаляем рейтинг
            await _rankingService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}