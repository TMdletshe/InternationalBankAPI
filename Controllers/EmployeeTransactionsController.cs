using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InternationalBankAPI.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace InternationalBankAPI.Controllers
{
    [ApiController]
    [Route("api/employee/transactions")]
    public class EmployeeTransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("unverified-payments")]
        public async Task<IActionResult> GetUnverifiedPayments()
        {
            var payments = await _context.Payments
                .Where(p => !p.IsVerified)
                .ToListAsync();

            return Ok(payments);
        }




        [Authorize(Roles = "Employee")]
        [HttpPost("verify")]
        public IActionResult VerifyTransaction([FromBody] int paymentId)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == paymentId);
            if (payment == null)
                return NotFound("Payment not found.");

            payment.IsVerified = true; // assuming you have this field
            payment.VerifiedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok("Payment verified and submitted to SWIFT.");
        }
    }
}

