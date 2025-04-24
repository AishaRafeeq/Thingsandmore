using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cartopia.Data; 
using Cartopia.Models; 
using Cartopia.ViewModels; 
using System.Linq;
using System.Threading.Tasks;
using static AdminDashboardViewModel;

[Authorize] 
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    

     
        public IActionResult Search(string searchTerm)
        {
       
            ViewData["searchTerm"] = searchTerm;

       
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            return View(products.ToList());
        }

 
   



    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Address = model.Address,
                City = model.City,
                Country = model.Country
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
               
                await _userManager.AddToRoleAsync(user, "Customer");

                return RedirectToAction("Login", "Account");
            }

      
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

           
            if (await _userManager.IsInRoleAsync(user!, "Admin"))
            {
                return RedirectToAction("Dashboard", "Admin"); 
            }
            else
            {
                return RedirectToAction("Dashboard", "Account"); 
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }




    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }
        
        
        

        
    

  
    var categories = new List<string> { "Fashion", "Electronics", "Home", "Beauty" };

   
        var allProducts = await _context.Products.ToListAsync();


        var featuredProducts = allProducts
            .GroupBy(p => p.Category)
            .SelectMany(g => g.Take(5))
            .ToList();

 
        ViewBag.Categories = categories;
        ViewBag.FeaturedProducts = featuredProducts;

        try
        {
            var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/carrousal");
            if (Directory.Exists(imageFolderPath))
            {
                var imageFiles = Directory.GetFiles(imageFolderPath).Select(Path.GetFileName).ToList();
                ViewBag.CarouselImages = imageFiles;
            }
            else
            {
                ViewBag.CarouselImages = new List<string>(); 
            }
        }
        catch (Exception ex)
        {
         
            Console.WriteLine($"Error fetching carousel images: {ex.Message}");
            ViewBag.CarouselImages = new List<string>(); 
        }

        var dashboardData = new CustomerDashboardViewModel
        {
            FullName = user.FullName,
            Email = user.Email!,
            RecentOrders = new List<OrderViewModel>(), 
            Products = new List<Product>(), 
            Categories = categories

        };

        return View(dashboardData);
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ViewProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

      
        var model = new EditProfileViewModel
        {
            FullName = user.FullName,
            Address = user.Address,
            City = user.City, 
            Country = user.Country 
        };

        return View(model);
    }



    
    [HttpGet]
    [Authorize] 
    public async Task<IActionResult> EditProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var model = new EditProfileViewModel
        {
            FullName = user.FullName,
            Address = user.Address,
            City = user.City,
            Country = user.Country
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] 
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            user.FullName = model.FullName;
            user.Address = model.Address;
            user.City = model.City;
            user.Country = model.Country;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("ViewProfile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }
    
        [HttpGet]
        public IActionResult SearchProduct(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                
                return RedirectToAction(nameof(Dashboard));
            }

            
            var relatedProducts = _context.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToList();

        
            return View("SearchResults", relatedProducts);

        }


        
        

   
    [HttpGet]
    [Authorize] 
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize] 
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Dashboard");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }
}