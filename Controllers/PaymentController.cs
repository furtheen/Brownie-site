using Microsoft.AspNetCore.Mvc;
using BrownieShop.API.Models;
using BrownieShop.API.Data;
using BrownieShop.API.Services;
using BrownieShop.API.DTOs;
using System.Threading.Tasks;

namespace BrownieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PaymentService _paymentService;

        public PaymentController(ApplicationDbContext context, PaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        [HttpPost("create-order/{orderId}")]
        public async Task<IActionResult> CreateRazorpayOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            try
            {
                string rzpayOrderId = _paymentService.CreateRazorpayOrder(order.Total, order.Id.ToString());
                return Ok(new { RazorpayOrderId = rzpayOrderId, Amount = order.Total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to create Razorpay order", Details = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerificationRequest request)
        {
            bool isValid = _paymentService.VerifySignature(
                request.RazorpayOrderId, 
                request.RazorpayPaymentId, 
                request.RazorpaySignature);

            if (isValid)
            {
                // Payment successful. We can optionally mark the Order as Paid in the DB.
                // Assuming we add a 'Status' field later, we would update it here:
                // var order = await _context.Orders.FindAsync(request.LocalOrderId);
                // order.Status = "Paid";
                // await _context.SaveChangesAsync();

                return Ok(new { Message = "Payment verifying successfully!" });
            }

            return BadRequest("Invalid Payment Signature");
        }
    }
}
