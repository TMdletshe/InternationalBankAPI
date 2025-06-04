namespace InternationalBankAPI.Models
{
    public class LoginDto
    {
        public required string AccountNumber { get; set; }
        public required string Password { get; set; }
    }
}

