using KooliProjekt.Services;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        [FromQuery] string teamName = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] int? tournamentId = null)
    {
        // Создание объекта поиска
        var search = new MatchSearch
        {
            TeamName = teamName,
            DateFrom = dateFrom,
            DateTo = dateTo,
            TournamentId = tournamentId
        };

        // Получение результата с пагинацией и фильтрацией
        var result = await _matchService.List(page, pageSize, search);

        // Возвращение результата в формате Ok с пагинированными данными
        return Ok(result);
    }
}
