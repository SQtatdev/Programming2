using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class PredictionsController : Controller
    {
        private readonly IPredictionService _predictionService;

        public PredictionsController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        // Метод для отображения списка предсказаний с пагинацией
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            // Получаем данные с пагинацией
            var data = await _predictionService.List(page, pageSize);
            return View(data);  // Передаем данные в представление
        }

        // Метод для отображения деталей предсказания
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Получаем предсказание по ID
            var prediction = await _predictionService.GetById(id.Value);
            return prediction == null ? NotFound() : View(prediction);
        }

        // Метод для отображения формы создания предсказания
        public IActionResult Create()
        {
            return View();
        }

        // Метод для обработки данных и создания предсказания
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MatchId,UserId,PredictedScoreFirstTeam,PredictedScoreSecondTeam,Punktid")] Prediction prediction)
        {
            if (!ModelState.IsValid) return View(prediction);

            // Сохраняем предсказание
            await _predictionService.Save(prediction);
            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы редактирования предсказания
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Получаем предсказание по ID
            var prediction = await _predictionService.GetById(id.Value);
            return prediction == null ? NotFound() : View(prediction);
        }

        // Метод для обработки редактирования предсказания
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MatchId,UserId,PredictedScoreFirstTeam,PredictedScoreSecondTeam,Punktid")] Prediction prediction)
        {
            if (id != prediction.Id) return NotFound();
            if (!ModelState.IsValid) return View(prediction);

            try
            {
                // Редактируем предсказание
                await _predictionService.Edit(prediction);
            }
            catch
            {
                if (!await _predictionService.Exists(prediction.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы удаления предсказания
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Получаем предсказание по ID
            var prediction = await _predictionService.GetById(id.Value);
            return prediction == null ? NotFound() : View(prediction);
        }

        // Метод для удаления предсказания
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Удаляем предсказание
            await _predictionService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
