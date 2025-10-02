namespace ECommerceAPI.DTOs
{
    public class WishlistCreateDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
