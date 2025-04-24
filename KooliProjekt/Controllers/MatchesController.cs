using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly ApplicationDbContext _context;

        public MatchesController(IMatchService matchService, ApplicationDbContext context)
        {
            _matchService = matchService;
            _context = context;
        }

        // GET: api/Matches
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] MatchSearch? search = null)
        {
            var matches = await _matchService.List(page, pageSize, search);
            return Ok(matches);
        }

        // GET api/Matches/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var match = await _matchService.GetById(id);

            if (match == null)
            {
                return NotFound();
            }

            return Ok(match);
        }

        // POST api/Matches
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _matchService.Save(match);
            return CreatedAtAction(nameof(GetById), new { id = match.Id }, match);
        }

        // PUT api/Matches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Match match)
        {
            if (id != match.Id)
            {
                return BadRequest();
            }

            try
            {
                await _matchService.Edit(match);
            }
            catch
            {
                if (!await _matchService.Exists(match.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE api/Matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _matchService.Delete(id);

            return RedirectToAction(nameof(Index));

            var item = await _matchService.GetById(id);
            if(item == null)
            {
                return NotFound();
            }

            await _matchService.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}