public class OrderSummaryDto
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "";
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
}
