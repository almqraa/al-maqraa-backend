using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Al_Maqraa.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SurahController : ControllerBase
    {
        private readonly SurahService _service;

        public SurahController(SurahService service)
        {
            _service = service;
        }
        // GET: api/Surah
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Surah>>> GetSurah()
        {
            var surah = await _service.GetAllAsync();
            if (surah == null)
            {
                return NotFound();
            }
            return surah.ToList();
        }

        // GET: api/Surah/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Surah>> GetSurah(int id)
        {
            var surah = await _service.GetAllAsync();

            if (surah == null)
            {
                return NotFound();
            }
            var Surah = await _service.GetByIdAsync(id);
            if (Surah == null)
            {
                return NotFound();
            }

            return Surah;
        }

        // PUT: api/Surah/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurah(int id, Surah Surah)
        {
            if (id != Surah.Id)
            {
                return BadRequest();
            }


            try
            {
                await _service.UpdateAsync(Surah);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SurahExistsAsync(id))
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

        // POST: api/Surah
        [HttpPost]
        public async Task<ActionResult<Surah>> PostSurah(Surah Surah)
        {
            var surah = await _service.GetAllAsync();

            if (surah == null)
            {
                return Problem("Entity set 'Surah'  is null.");
            }
            await _service.AddAsync(Surah);
            return CreatedAtAction("GetSurah", new { id = Surah.Id }, Surah);
        }

        // DELETE: api/Surah/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurah(int id)
        {
            var surah = await _service.GetAllAsync();

            if (surah == null)
            {
                return NotFound();
            }

            var Surah = await _service.GetByIdAsync(id);
            if (Surah == null)
            {
                return NotFound();
            }

            await _service.DeleteAsync(Surah.Id);

            return NoContent();
        }

        private async Task<bool> SurahExistsAsync(int id)
        {
            var surahs =  await _service.GetAllAsync();

            return (surahs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
