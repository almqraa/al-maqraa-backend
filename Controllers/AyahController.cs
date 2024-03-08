using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Al_Maqraa.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AyahController : ControllerBase
    {
        private readonly AyahService _service;

        public AyahController(AyahService service)
        {
            _service = service;
        }

        // GET: api/Ayah
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ayah>>> GetAyah()
        {
            var user = await _service.GetAllAsync();
            if (user == null)
            {
                return NotFound();
            }
            return user.ToList();
        }

        // GET: api/Ayah/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ayah>> GetAyah(int id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }
            var Ayah = await _service.GetByIdAsync(id);
            if (Ayah == null)
            {
                return NotFound();
            }

            return Ayah;
        }

        // PUT: api/Ayah/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAyah(int id, Ayah Ayah)
        {
            if (id != Ayah.Id)
            {
                return BadRequest();
            }


            try
            {
                await _service.UpdateAsync(Ayah);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AyahExistsAsync(id))
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

        // POST: api/Ayah
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ayah>> PostAyah(Ayah Ayah)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return Problem("Entity set 'Ayah'  is null.");
            }
            await _service.AddAsync(Ayah);
            return CreatedAtAction("GetAyah", new { id = Ayah.Id }, Ayah);
        }

        // DELETE: api/Ayah/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAyah(int id)
        {
            var user = await _service.GetAllAsync();

            if (user == null)
            {
                return NotFound();
            }

            var Ayah = await _service.GetByIdAsync(id);
            if (Ayah == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(Ayah.Id);

            return NoContent();
        }

        private async Task<bool> AyahExistsAsync(int id)
        {
            var users =  await _service.GetAllAsync();

            return (users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
