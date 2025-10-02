namespace ECommerceAPI.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }  // PK
        public int WishlistId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; }
    }
}
