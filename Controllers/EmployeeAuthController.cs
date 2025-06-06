using InternationalBankAPI.Data;
using InternationalBankAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace InternationalBankAPI.Controllers
{

    [ApiController]
    [Route("api/employeeauth")]
    public class EmployeeAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public EmployeeAuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(EmployeeLoginDto request)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Username == request.Username);
            if (employee == null || !VerifyPasswordHash(request.Password, employee.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = CreateToken(employee);
            return Ok(new { token });
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            throw new NotImplementedException();
        }

        private string CreateToken(Employee employee)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, employee.Username),
            new Claim(ClaimTypes.Role, "Employee")
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
