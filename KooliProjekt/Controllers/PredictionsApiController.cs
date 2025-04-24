using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionsApiController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionsApiController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        // GET: api/Predictions
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var predictions = await _predictionService.List(page, pageSize);
            return Ok(predictions);
        }

        // GET api/Predictions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prediction = await _predictionService.GetById(id);

            if (prediction == null)
            {
                return NotFound();
            }

            return Ok(prediction);
        }

        // POST api/Predictions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Prediction prediction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _predictionService.Save(prediction);
            return CreatedAtAction(nameof(GetById), new { id = prediction.Id }, prediction);
        }

        // PUT api/Predictions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Prediction prediction)
        {
            if (id != prediction.Id)
            {
                return BadRequest();
            }

            try
            {
                await _predictionService.Edit(prediction);
            }
            catch
            {
                if (!await _predictionService.Exists(prediction.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE api/Predictions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _predictionService.Delete(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}