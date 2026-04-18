using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrownieShop.API.Models;
using BrownieShop.API.Data;

namespace BrownieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BrowniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/brownies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brownie>>> GetBrownies()
        {
            return await _context.Brownies.ToListAsync();
        }

        // GET: api/brownies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brownie>> GetBrownie(int id)
        {
            var brownie = await _context.Brownies.FindAsync(id);

            if (brownie == null)
            {
                return NotFound();
            }

            return brownie;
        }

        // POST: api/brownies  (Admin function)
        [HttpPost]
        public async Task<ActionResult<Brownie>> PostBrownie(Brownie brownie)
        {
            _context.Brownies.Add(brownie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrownie", new { id = brownie.Id }, brownie);
        }

        // PUT: api/brownies/5 (Admin function)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrownie(int id, Brownie brownie)
        {
            if (id != brownie.Id)
            {
                return BadRequest();
            }

            _context.Entry(brownie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrownieExists(id))
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

        // DELETE: api/brownies/5 (Admin function)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrownie(int id)
        {
            var brownie = await _context.Brownies.FindAsync(id);
            if (brownie == null)
            {
                return NotFound();
            }

            _context.Brownies.Remove(brownie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrownieExists(int id)
        {
            return _context.Brownies.Any(e => e.Id == id);
        }
    }
}
