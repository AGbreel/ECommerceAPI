public class OrderDetailDto
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime OrderDate { get; set; }

    // 🟢 User Info
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";

    // 🟢 Items
    public List<OrderItemDto> Items { get; set; } = new();

    // 🟢 Shipping
    public OrderShippingDto? Shipping { get; set; }
}

public class OrderItemDto
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class OrderShippingDto
{
    public int OrderId { get; set; }
    public int ShippingId { get; set; }
    public string RecipientName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string AddressLine1 { get; set; } = "";
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Notes { get; set; }
}
