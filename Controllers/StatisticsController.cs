using Al_Maqraa.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

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
        public async Task<IActionResult> PutStatistics(int id, StatisticsDTO statisticsDTO)
        {
            Statistics statistics = await _service.GetByIdAsync(id);
            if (statistics==null)
            {
                return BadRequest();
            }
            try
            {
                statistics.Bookmark = statisticsDTO.Bookmark ?? statistics.Bookmark;
                statistics.DayStreak = statisticsDTO.DayStreak ?? statistics.DayStreak;
                statistics.LastRead = statisticsDTO.LastRead ?? statistics.LastRead;
                if (statisticsDTO.TotalReadingTime != null) statistics.TotalReadingTime += statisticsDTO.TotalReadingTime;
                await _service.UpdateAsync(statistics);
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
        public async Task<ActionResult<Statistics>> PostStatistics(Statistics statistics)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return Problem("Entity set 'Statistics'  is null.");
            }
            string userId = statistics.UserId;
            Statistics? userStatistics = await _service.CheckStatisticsByUserId(userId);
            if (userStatistics != null)
            {
                userStatistics.TotalReadingTime += statistics.TotalReadingTime;
                await _service.UpdateAsync(userStatistics);
                return Content("Statistics Updated");
            }
            await _service.AddAsync(statistics);
            return CreatedAtAction("GetStatistics", new { id = statistics.Id }, statistics);
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
        [HttpGet("user/{statisticId}")]
        public async Task<IActionResult> GetUserByStatisticId(int statisticId)
        {
  
            // Retrieve the user associated with the statistic
             var user = await _service.GetUserByStatisticId(statisticId);
            //Statistics statistic = await _context.Statistics.Include(s =>s.User).FirstOrDefaultAsync(ss =>ss.Id==statisticId);
            //var user = statistic.User;
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
    }
}
