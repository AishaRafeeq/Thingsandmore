using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cartopia.Data;
using Cartopia.Models;
using Cartopia.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Cartopia.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetCartId()
        {
            var cartId = HttpContext.Session.GetString("CartID");
            if (string.IsNullOrEmpty(cartId))
            {
                cartId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartID", cartId);  
            }
            return cartId;
        }

        public async Task<IActionResult> Index()
        {
            var cartId = GetCartId();
            var cartItems = await _context.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CartID == cartId)
                .ToListAsync();

            if (cartItems.Any())
            {
                ViewBag.Total = cartItems.Sum(c => c.Product.Price * c.Quantity);
            }
            else
            {
                ViewBag.Total = 0;
            }

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var cartId = GetCartId();
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = _context.ShoppingCartItems
                .FirstOrDefault(c => c.CartID == cartId && c.ProductID == productId);

            if (cartItem == null)
            {
                cartItem = new ShoppingCartItem
                {
                    CartID = cartId,
                    ProductID = productId,
                    Product = product,
                    Quantity = quantity
                };
                _context.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cartId = GetCartId();
            var cartItem = _context.ShoppingCartItems
                .FirstOrDefault(c => c.ProductID == productId && c.CartID == cartId);

            if (cartItem != null)
            {
                _context.ShoppingCartItems.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index)); 
        }

        
        public async Task<IActionResult> Checkout()
        {
            var cartId = GetCartId();
            var cartItems = await _context.ShoppingCartItems
                .Include(c => c.Product)
                .Where(c => c.CartID == cartId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);

            var checkoutViewModel = new CheckoutViewModel
            {
                OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    ProductID = c.ProductID,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                }).ToList(),
                OrderTotal = totalAmount,
                ShippingAddress = string.Empty 
            };

            return View(checkoutViewModel);
        }

        
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout", viewModel); 
            }

            // Create the Order
            var order = new Order
            {
                UserID = User.Identity?.Name,
                OrderDate = DateTime.Now,
                ShippingAddress = viewModel.ShippingAddress,
                TotalAmount = viewModel.OrderTotal,
                OrderDetails = viewModel.OrderDetails?.Select(od => new OrderDetail
                {
                    ProductID = od.ProductID,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

           
            var cartId = GetCartId();
            var cartItems = _context.ShoppingCartItems.Where(c => c.CartID == cartId).ToList();
            _context.ShoppingCartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            
            return RedirectToAction("OrderSummary", new { orderId = order.OrderID });
        }
        
        

    }
}

