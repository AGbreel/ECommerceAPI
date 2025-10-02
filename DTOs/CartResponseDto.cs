using System.Collections.Generic;

namespace ECommerceAPI.DTOs
{
    public class CartResponseDto
    {
        public IEnumerable<CartProductDto> Items { get; set; }
        public decimal CartTotal { get; set; }
    }
}
