namespace InternationalBankAPI.Models
{
    public class CustomerDto
    {
        public required string FullName { get; set; }
        public required string IDNumber { get; set; }
        public required string AccountNumber { get; set; }
        public required string Password { get; set; }
    }
}
