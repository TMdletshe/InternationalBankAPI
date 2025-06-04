
namespace InternationalBankAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string IDNumber { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}


