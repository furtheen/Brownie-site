using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrownieShop.API.Models;
using BrownieShop.API.Data;
using BrownieShop.API.DTOs;

namespace BrownieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Brownie)
                .ToListAsync();
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(CreateOrderRequest request, [FromServices] Services.EmailService emailService)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return BadRequest("Order requires at least one item.");
            }

            var order = new Order
            {
                CustomerName = request.CustomerName,
                Date = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };

            decimal total = 0;
            var productNames = new List<string>();

            foreach (var itemDto in request.Items)
            {
                var brownie = await _context.Brownies.FindAsync(itemDto.BrownieId);
                if (brownie == null)
                {
                    return BadRequest($"Brownie with Id {itemDto.BrownieId} not found.");
                }

                var orderItem = new OrderItem
                {
                    BrownieId = brownie.Id,
                    Quantity = itemDto.Quantity
                };

                order.Items.Add(orderItem);
                total += brownie.Price * itemDto.Quantity;
                productNames.Add($"{itemDto.Quantity}x {brownie.Name}");
            }

            order.Total = total;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Background email send
            try
            {
                var targetEmail = string.IsNullOrEmpty(request.Email) ? "furtheen23@gmail.com" : request.Email;
                var productsStr = string.Join(", ", productNames);
                await emailService.SendEmail(
                    targetEmail,
                    "Order Confirmed 🍫",
                    $"Hi {request.CustomerName}, your order for {productsStr} is confirmed! Total: ₹{total}."
                );
            }
            catch
            {
                // Optionally log here, but don't stop the order
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Brownie)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
