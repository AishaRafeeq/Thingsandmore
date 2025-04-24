namespace Cartopia.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ShoppingCartItem
    {
        [Key]
        public int ShoppingCartItemID { get; set; }

        [Required]
        public int ProductID { get; set; }
        public required Product Product { get; set; } 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public required string CartID { get; set; } 
    }
}
