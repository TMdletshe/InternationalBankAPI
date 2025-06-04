namespace InternationalBankAPI.Models
{

    public class Payment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Provider { get; set; } = string.Empty;
        public string SwiftCode { get; set; } = string.Empty;
        public string RecipientAccount { get; set; } = string.Empty;
        public bool IsVerified { get; set; } = false;
       
        public DateTime? VerifiedAt { get; set; }
        public bool SentToSWIFT { get; set; }
        public DateTime? SentAt { get; set; }






        public  Customer? Customer { get; set; }
    }
}
