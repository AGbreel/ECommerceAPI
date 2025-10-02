namespace ECommerceAPI.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string Method { get; set; } = "COD"; // Cash on Delivery
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
