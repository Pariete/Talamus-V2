namespace Talamus_V2.Net6.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int BookId { get; set; }
        public string PaymentId { get; set; }
        public bool Confirmed { get; set; }
    }
}
