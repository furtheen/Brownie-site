using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrownieShop.API.Data;
using BrownieShop.API.Models;

namespace BrownieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserCart(string userId)
        {
            var items = await _context.CartItems
                .Include(c => c.Brownie)
                .Where(c => c.UserId == userId)
                .Select(c => new
                {
                    id = c.BrownieId,
                    name = c.Brownie != null ? c.Brownie.Name : "Unknown",
                    emoji = c.Brownie != null ? c.Brownie.ImageUrl : "🦇",
                    price = c.Brownie != null ? c.Brownie.Price : 0,
                    qty = c.Quantity
                })
                .ToListAsync();

            return Ok(items);
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] CartItemRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || request.BrownieId <= 0)
                return BadRequest("Invalid cart request");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.BrownieId == request.BrownieId);

            if (existingItem != null)
            {
                existingItem.Quantity = request.Quantity; // Overwrite absolute quantity
            }
            else
            {
                var brownieExists = await _context.Brownies.AnyAsync(b => b.Id == request.BrownieId);
                if (!brownieExists) return NotFound("Brownie not found");

                _context.CartItems.Add(new CartItem
                {
                    UserId = request.UserId,
                    BrownieId = request.BrownieId,
                    Quantity = request.Quantity
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Cart/{userId}/{brownieId}
        [HttpDelete("{userId}/{brownieId}")]
        public async Task<IActionResult> RemoveCartItem(string userId, int brownieId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BrownieId == brownieId);
            
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        // DELETE: api/Cart/{userId}/clear
        [HttpDelete("{userId}/clear")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var items = await _context.CartItems.Where(c => c.UserId == userId).ToListAsync();
            if (items.Any())
            {
                _context.CartItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }

    public class CartItemRequest
    {
        public string UserId { get; set; } = string.Empty;
        public int BrownieId { get; set; }
        public int Quantity { get; set; }
    }
}
