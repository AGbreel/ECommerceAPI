namespace ECommerceAPI.Models
{
    public class OrderShipping
    {
        public int ShippingId  { get; set; }
        public int OrderId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime ShippedDate { get; set; }
    }
}
