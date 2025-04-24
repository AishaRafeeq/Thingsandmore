using Cartopia.ViewModels;
using System.Collections.Generic;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalRoles { get; set; }
    public int TotalOrders { get; set; }
    public List<string> RecentUsers { get; set; } = new List<string>(); 
    public List<string> RecentRoles { get; set; } = new List<string>(); 
    public List<OrderViewModel> RecentOrders { get; set; } = new List<OrderViewModel>();
    

   
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
