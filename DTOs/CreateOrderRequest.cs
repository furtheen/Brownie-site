namespace BrownieShop.API.DTOs
{
    public class CreateOrderRequest
    {
        public string CustomerName { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }

    public class CartItemDTO
    {
        public int BrownieId { get; set; }
        public int Quantity { get; set; }
    }
}
