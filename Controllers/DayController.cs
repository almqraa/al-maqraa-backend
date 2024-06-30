
using Al_Maqraa.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
namespace Al_Maqraa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayController : ControllerBase
    {
        private readonly DayService _service;
      
        public DayController(DayService service)
        {
            _service = service;
        }

        // GET: api/Day
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Day>>> GetDay()
        {
            var days = await _service.GetAllAsync();
            if (days == null)
            {
                return NotFound();
            }
            return days.ToList();
        }

        // GET: api/Day/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Day>> GetDay(int id)
        {
            var days = await _service.GetAllAsync();

            if (days == null)
            {
                return NotFound();
            }
            var Day = await _service.GetByIdAsync(id);
            if (Day == null)
            {
                return NotFound();
            }

            return Day;
        }

        // PUT: api/Day/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDay(int id, DayDTO dayDTO)
        {
            Day day = await _service.GetByIdAsync(id);
            if (day == null)
            {
                return BadRequest();
            }

            try
            {
                day.Date = dayDTO.Date ?? day.Date;
                day.Score = dayDTO.Score ?? day.Score;
                day.UserId = dayDTO.UserId ?? day.UserId;
                await _service.UpdateAsync(day);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DayExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Content("Day Updated");
        }

        // POST: api/Day
        [HttpPost]
        public async Task<ActionResult<Day>> PostDay(Day Day)
        {
            var days = await _service.GetAllAsync();

            if (days == null)
            {
                return Problem("Entity set 'Day'  is null.");
            }
            string userId = Day.UserId;
            Day userDay = await _service.CheckDayByUserId(userId);
            if (userDay != null)
            {
                userDay.Score += Day.Score;
                await _service.UpdateAsync(userDay);
                return Content("Day Updated");

            }
            await _service.AddAsync(Day);
            return CreatedAtAction("GetDay", new { id = Day.Id }, Day);
        }

        // DELETE: api/Day/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDay(int id)
        {
            var days = await _service.GetAllAsync();

            if (days == null)
            {
                return NotFound();
            }

            var Day = await _service.GetByIdAsync(id);
            if (Day == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(Day.Id);

            return NoContent();
        }

        private async Task<bool> DayExistsAsync(int id)
        {
            var days =  await _service.GetAllAsync();

            return (days?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [HttpGet("user/{dayId}")]
        public async Task<IActionResult> GetUserByDayId(int dayId)
        {
  
            // Retrieve the user associated with the statistic
             var user = await _service.GetUserByDayId(dayId);
            //Day statistic = await _context.Day.Include(s =>s.User).FirstOrDefaultAsync(ss =>ss.Id==statisticId);
            //var user = statistic.User;
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }


    }
}
