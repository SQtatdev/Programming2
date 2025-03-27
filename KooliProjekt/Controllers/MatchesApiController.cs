using KooliProjekt.Services;
using KooliProjekt.Models;
using KooliProjekt.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesApiController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesApiController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMatches(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string? teamName = null,  // Добавлен nullable тип
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] DateTime? dateTo = null,
            [FromQuery] int? tournamentId = null)
        {
            try
            {
                // Создание объекта поиска
                var search = new MatchSearch
                {
                    TeamName = teamName,
                    StartDate = dateFrom,
                    EndDate = dateTo,
                    TournamentId = tournamentId
                };

                // Валидация параметров
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                // Получение данных
                var result = await _matchService.List(page, pageSize, search);

                // Возвращение результата
                return Ok(new
                {
                    Data = result.Items,
                    result.TotalCount,
                    result.Page,
                    result.PageSize,
                    result.PageCount
                });
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                return StatusCode(500, new
                {
                    Error = "Internal server error",
                    ex.Message
                });
            }
        }
    }
}