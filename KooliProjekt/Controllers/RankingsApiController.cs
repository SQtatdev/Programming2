using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingsApiController : ControllerBase
    {
        private readonly IRankingServices _rankingService;

        public RankingsApiController(IRankingServices rankingService)
        {
            _rankingService = rankingService;
        }

        // GET: api/Rankings
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var rankings = await _rankingService.List(page, pageSize);
            return Ok(rankings);
        }

        // GET api/Rankings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ranking = await _rankingService.GetById(id);

            if (ranking == null)
            {
                return NotFound();
            }

            return Ok(ranking);
        }

        // POST api/Rankings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ranking ranking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _rankingService.Save(ranking);
            return CreatedAtAction(nameof(GetById), new { id = ranking.Id }, ranking);
        }

        // PUT api/Rankings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ranking ranking)
        {
            if (id != ranking.Id)
            {
                return BadRequest();
            }

            try
            {
                await _rankingService.Edit(ranking);
            }
            catch
            {
                if (!await _rankingService.Exists(ranking.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE api/Rankings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _rankingService.Delete(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST api/Rankings/update-all
        [HttpPost("update-all")]
        public async Task<IActionResult> UpdateAllRankings()
        {
            await _rankingService.UpdateAllRankings();
            return Ok();
        }
    }
}