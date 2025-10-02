namespace ECommerceAPI.Models
{
    public class OrderStatusHistory
    {
        public int Id { get; set; }  // PK
        public int HistoryId { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ChangedBy { get; set; } // AdminId لو متاح
        public string? Note { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
