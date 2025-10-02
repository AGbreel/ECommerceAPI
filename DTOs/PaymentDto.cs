namespace ECommerceAPI.DTOs
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public string Method { get; set; } = "CashOnDelivery"; // حالياً الدفع عند الاستلام
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Paid, Failed
    }
}
