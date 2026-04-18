using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BrownieShop.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int BrownieId { get; set; }
        
        [JsonIgnore]
        public Brownie? Brownie { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
