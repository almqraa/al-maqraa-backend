using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Al_Maqraa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SheikhController : ControllerBase
    {
        private readonly SheikhService _service;

        public SheikhController(SheikhService service)
        {
            _service = service;
        }

        // GET: api/Sheikh
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sheikh>>> GetSheikh()
        {
            var user = await _service.GetAllAsync();
            if (user == null)
            {
                return NotFound();
            }
            return user.ToList();
        }

        // GET: api/Sheikh/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sheikh>> GetSheikh(int id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }
            var Sheikh = await _service.GetByIdAsync(id);
            if (Sheikh == null)
            {
                return NotFound();
            }

            return Sheikh;
        }

        // PUT: api/Sheikh/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSheikh(int id, Sheikh Sheikh)
        {
            if (id != Sheikh.Id)
            {
                return BadRequest();
            }


            try
            {
                await _service.UpdateAsync(Sheikh);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SheikhExistsAsync(id))
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

        // POST: api/Sheikh
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sheikh>> PostSheikh(Sheikh Sheikh)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return Problem("Entity set 'Sheikh'  is null.");
            }
            await _service.AddAsync(Sheikh);
            return CreatedAtAction("GetSheikh", new { id = Sheikh.Id }, Sheikh);
        }

        // DELETE: api/Sheikh/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSheikh(int id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }

            var Sheikh = await _service.GetByIdAsync(id);
            if (Sheikh == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(Sheikh.Id);

            return NoContent();
        }

        private async Task<bool> SheikhExistsAsync(int id)
        {
            var users =  await _service.GetAllAsync();

            return (users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
