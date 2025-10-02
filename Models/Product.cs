namespace ECommerceAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        // public string Name { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<string> Colors { get; set; } = new();
        public List<string> Sizes { get; set; } = new();
        public List<string> Images { get; set; } = new();
        public decimal? PriceAfterDiscount { get; set; }
        public decimal RatingsAverage { get; set; }
    }
}
