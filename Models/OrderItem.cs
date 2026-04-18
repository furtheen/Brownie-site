namespace BrownieShop.API.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public Order Order { get; set; }
        
        public int BrownieId { get; set; }
        public Brownie Brownie { get; set; }
        
        public int Quantity { get; set; }
    }
}
