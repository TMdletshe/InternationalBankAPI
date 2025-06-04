using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternationalBankAPI.Data;
using InternationalBankAPI.Models;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace InternationalBankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerDto request)
        {
            // Input validation with RegEx
            if (!Regex.IsMatch(request.FullName, @"^[a-zA-Z\s]{2,}$"))
                return BadRequest("Full name is invalid.");
            if (!Regex.IsMatch(request.IDNumber, @"^\d{13}$"))
                return BadRequest("ID number must be 13 digits.");
            if (!Regex.IsMatch(request.AccountNumber, @"^\d{10,20}$"))
                return BadRequest("Account number is invalid.");
            if (request.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters.");

            // Check for existing account
            if (await _context.Customers.AnyAsync(c => c.AccountNumber == request.AccountNumber))
                return BadRequest("Account already exists.");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var customer = new Customer
            {
                FullName = request.FullName,
                IDNumber = request.IDNumber,
                AccountNumber = request.AccountNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.AccountNumber == request.AccountNumber);
            if (customer == null) return Unauthorized("Invalid credentials.");

            if (!VerifyPasswordHash(request.Password, customer.PasswordHash, customer.PasswordSalt))
                return Unauthorized("Invalid credentials.");

            // Generate JWT token
            var token = CreateToken(customer);
            return Ok(new { token });

           
        }

        private string CreateToken(Customer customer)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
        new Claim(ClaimTypes.Name, customer.AccountNumber)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Helper Methods
        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computed.SequenceEqual(hash);
        }
    }
}

