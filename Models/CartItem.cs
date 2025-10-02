namespace ECommerceAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }  // PK
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
