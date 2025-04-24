namespace Cartopia.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public  string? UserID { get; set; }
        public ApplicationUser? User { get; set; } 

        [Required]
        public string? ShippingAddress { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }

        public  ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
