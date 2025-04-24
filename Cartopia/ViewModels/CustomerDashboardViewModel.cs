namespace Cartopia.ViewModels
{
    using Cartopia.Models;
    using System.Collections.Generic;

    public class CustomerDashboardViewModel
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int TotalOrders { get; set; }
        public required List<OrderViewModel> RecentOrders { get; set; }
        public   required List<string> Categories { get; set; } 
        public  required List<Product> Products { get; set; }
        
         
        

    }
}
