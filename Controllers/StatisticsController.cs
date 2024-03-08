using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Al_Maqraa.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _service;

        public StatisticsController(StatisticsService service)
        {
            _service = service;
        }

        // GET: api/Statistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Statistics>>> GetStatistics()
        {
            var user = await _service.GetAllAsync();
            if (user == null)
            {
                return NotFound();
            }
            return user.ToList();
        }

        // GET: api/Statistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Statistics>> GetStatistics(int id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }
            var Statistics = await _service.GetByIdAsync(id);
            if (Statistics == null)
            {
                return NotFound();
            }

            return Statistics;
        }

        // PUT: api/Statistics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatistics(int id, Statistics Statistics)
        {
            if (id != Statistics.Id)
            {
                return BadRequest();
            }


            try
            {
                await _service.UpdateAsync(Statistics);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StatisticsExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Statistics
        [HttpPost]
        public async Task<ActionResult<Statistics>> PostStatistics(Statistics Statistics)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return Problem("Entity set 'Statistics'  is null.");
            }
            await _service.AddAsync(Statistics);
            return CreatedAtAction("GetStatistics", new { id = Statistics.Id }, Statistics);
        }

        // DELETE: api/Statistics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatistics(int id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }

            var Statistics = await _service.GetByIdAsync(id);
            if (Statistics == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(Statistics.Id);

            return NoContent();
        }

        private async Task<bool> StatisticsExistsAsync(int id)
        {
            var users =  await _service.GetAllAsync();

            return (users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
