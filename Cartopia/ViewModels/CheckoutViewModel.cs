using Cartopia.Models;

namespace Cartopia.ViewModels
{
    public class CheckoutViewModel
    {
       
        public List<OrderDetail>? OrderDetails { get; set; }

        
        public decimal OrderTotal { get; set; }

        
        public string? ShippingAddress { get; set; }

       
        public string? PhoneNumber { get; set; }

       
        public string ?PaymentMethod { get; set; }
    }
}
