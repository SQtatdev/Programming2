using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;

        // Конструктор для внедрения зависимости ITeamService
        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // Метод для отображения списка команд с пагинацией
        public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
        {
            // Получаем данные с пагинацией
            var data = await _teamService.List(page, pageSize);
            return View(data);  // Передаем данные в представление
        }

        // Метод для отображения деталей команды
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Получаем команду по ID
            var team = await _teamService.GetById(id.Value);
            return team == null ? NotFound() : View(team);
        }

        // Метод для отображения формы создания команды
        public IActionResult Create()
        {
            return View();
        }

        // Метод для обработки данных и создания команды
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamName,TeamPlayers")] Team team)
        {
            if (!ModelState.IsValid) return View(team);

            // Сохраняем команду
            await _teamService.Save(team);
            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы редактирования команды
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // Получаем команду по ID
            var team = await _teamService.GetById(id.Value);
            return team == null ? NotFound() : View(team);
        }

        // Метод для обработки редактирования команды
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName,TeamPlayers")] Team team)
        {
            if (id != team.Id) return NotFound();
            if (!ModelState.IsValid) return View(team);

            try
            {
                // Редактируем команду
                await _teamService.Edit(team);
            }
            catch
            {
                if (!await _teamService.Exists(team.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Метод для отображения формы удаления команды
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            // Получаем команду по ID
            var team = await _teamService.GetById(id.Value);
            return team == null ? NotFound() : View(team);
        }

        // Метод для удаления команды
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Удаляем команду
            await _teamService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}