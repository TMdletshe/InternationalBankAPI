using InternationalBankAPI.Data;
using InternationalBankAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InternationalBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendPayment(PaymentDto request)
        {
            // Get the currently authenticated customer ID from JWT
            var customerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Basic validation
            if (request.Amount <= 0 || string.IsNullOrEmpty(request.Currency) || string.IsNullOrEmpty(request.RecipientAccount))
            {
                return BadRequest("Invalid payment request.");
            }

            var payment = new Payment
            {
                CustomerId = customerId,
                Currency = request.Currency,
                Amount = request.Amount,
                Provider = request.Provider,
                SwiftCode = request.SwiftCode,
                RecipientAccount = request.RecipientAccount,
                IsVerified = false // default to not verified
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok("Payment submitted successfully.");
        }
    }

    public class PaymentDto
    {
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string SwiftCode { get; set; } = string.Empty;
        public string RecipientAccount { get; set; } = string.Empty;
    }
}
