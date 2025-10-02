namespace ECommerceAPI.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty; // ✅ fix warnings
        public string ProductName { get; set; } = string.Empty;  // ✅ fix warnings
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";         // ✅ fix warnings
        public DateTime CreatedAt { get; set; }
    }
}
