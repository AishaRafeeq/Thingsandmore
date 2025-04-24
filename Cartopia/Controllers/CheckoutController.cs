using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cartopia.Data;
using Cartopia.Models;
using Microsoft.Extensions.Logging; 
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cartopia.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CheckoutController> _logger; 

        public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CheckoutController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger; 
        }

        public IActionResult Index()
        {
            var cartId = HttpContext.Session.GetString("CartID");
            if (string.IsNullOrEmpty(cartId))
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            var cartItems = _context.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CartID == cartId)
                .ToList();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            
            var totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);
            var orderViewModel = new Order
            {
                OrderDetails = cartItems.Select(item => new OrderDetail
                {
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList(),
                TotalAmount = totalAmount
            };

            return View(orderViewModel); 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]


        
        public async Task<IActionResult> PlaceOrder(Order order)
        {
            var cartId = HttpContext.Session.GetString("CartID");

            
            if (string.IsNullOrEmpty(cartId))
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            
            var cartItems = await _context.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CartID == cartId)
                .ToListAsync();

            
            if (!cartItems.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "ShoppingCart");
            }

            
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "You must be logged in to place an order.";
                return RedirectToAction("Login", "Account");
            }

            
            if (string.IsNullOrWhiteSpace(order.ShippingAddress))
            {
                TempData["Error"] = "Please provide a shipping address.";
                return RedirectToAction(nameof(Index));
            }

            
            order.UserID = userId;
            order.OrderDate = DateTime.Now;
            order.TotalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);
            order.OrderDetails = cartItems.Select(item => new OrderDetail
            {
                ProductID = item.ProductID,
                Quantity = item.Quantity,
                Price = item.Product.Price
            }).ToList();

            
            try
            {
               
                _context.Orders.Add(order);

                
                _context.ShoppingCartItems.RemoveRange(cartItems);

                
                await _context.SaveChangesAsync();

                
                TempData["Success"] = "Your order has been placed successfully!";

                
                return RedirectToAction(nameof(OrderSummary), new { id = order.OrderID });
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error placing order: " + ex.Message);
                TempData["Error"] = "An error occurred while placing your order.";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]

        public async Task<IActionResult> OrderSummary(int id)
        {
            var userId = _userManager.GetUserId(User);

            
            var order = await _context.Orders
                .Where(o => o.UserID == userId && o.OrderID == id)
                .Include(o => o.OrderDetails!)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync();

           
            if (order == null)
            {
                ViewBag.Message = "You haven't placed any order yet.";
                return View();  
            }

           
            TempData["Success"] = "Your order has been placed successfully!";

            return View(order);
        }



    }

}   


