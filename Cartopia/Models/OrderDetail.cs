using System.ComponentModel.DataAnnotations;

namespace Cartopia.Models
{
    

    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public  Order? Order { get; set; }

        [Required]
        public int? ProductID { get; set; }
        public    Product? Product { get; set; } 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
