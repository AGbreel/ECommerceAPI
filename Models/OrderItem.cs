namespace ECommerceAPI.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; // علشان التفاصيل
        public string ImageUrl { get; set; } = string.Empty; // علشان التفاصيل
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal LineTotal { get; set; }
    }
}
