using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TA22KrasanLab2.Data;
using TA22KrasanLab2.Models;
using TA22KrasanLab2.DTO;

namespace TA22KrasanLab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PigsApiController : ControllerBase
    {
        private readonly FarmContext _context;

        // Ін'єкція контексту бази даних, який вже налаштований у вашому проекті
        public PigsApiController(FarmContext context)
        {
            _context = context;
        }

        // GET: api/PigsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PigDto>>> GetPigs()
        {
            // Проекція даних з моделі бази даних в DTO
            var pigs = await _context.Pigs
                .Select(p => new PigDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Weight = p.Weight,
                    Description = p.Description
                })
                .ToListAsync();

            return Ok(pigs);
        }

        // GET: api/PigsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PigDto>> GetPig(int id)
        {
            var pig = await _context.Pigs.FindAsync(id);

            if (pig == null)
            {
                return NotFound();
            }

            var pigDto = new PigDto
            {
                Id = pig.Id,
                Name = pig.Name,
                Weight = pig.Weight,
                Description = pig.Description
            };

            return Ok(pigDto);
        }

        // POST: api/PigsApi
        [HttpPost]
        public async Task<ActionResult<PigDto>> PostPig(CreateUpdatePigDto pigDto)
        {
            // Маппінг з DTO в сутність EF
            var pig = new Pig
            {
                Name = pigDto.Name,
                Weight = pigDto.Weight,
                Description = pigDto.Description
            };

            _context.Pigs.Add(pig);
            await _context.SaveChangesAsync();

            // Повертаємо створений об'єкт та шлях до нього
            return CreatedAtAction(nameof(GetPig), new { id = pig.Id },
                new PigDto
                {
                    Id = pig.Id,
                    Name = pig.Name,
                    Weight = pig.Weight,
                    Description = pig.Description
                });
        }

        // PUT: api/PigsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPig(int id, CreateUpdatePigDto pigDto)
        {
            var pig = await _context.Pigs.FindAsync(id);
            if (pig == null)
            {
                return NotFound();
            }

            // Оновлення полів
            pig.Name = pigDto.Name;
            pig.Weight = pigDto.Weight;
            pig.Description = pigDto.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PigExists(id))
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

        // DELETE: api/PigsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePig(int id)
        {
            var pig = await _context.Pigs.FindAsync(id);
            if (pig == null)
            {
                return NotFound();
            }

            _context.Pigs.Remove(pig);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PigExists(int id)
        {
            return _context.Pigs.Any(e => e.Id == id);
        }
    }
}
