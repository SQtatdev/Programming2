using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsApiController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentsApiController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService ?? throw new ArgumentNullException(nameof(tournamentService));
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            try
            {
                var tournaments = await _tournamentService.List(page, pageSize);
                return Ok(tournaments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var tournament = await _tournamentService.GetById(id);
                if (tournament == null)
                {
                    return NotFound();
                }
                return Ok(tournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/Tournaments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Tournament tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _tournamentService.Save(tournament);
                return CreatedAtAction(nameof(GetById), new { id = tournament.Id }, tournament);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/Tournaments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _tournamentService.Edit(tournament);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tournamentService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/Tournaments/check-overlap
        [HttpGet("check-overlap")]
        public IActionResult CheckOverlap([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            try
            {
                var hasOverlap = _tournamentService.HasOverlappingTournaments(start, end);
                return Ok(new { HasOverlap = hasOverlap });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}