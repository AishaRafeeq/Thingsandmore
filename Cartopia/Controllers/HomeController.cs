using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Cartopia.Models;


using System.Linq;
using Cartopia.Data;
using Microsoft.AspNetCore.Identity;



public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        if (!User?.Identity?.IsAuthenticated ?? false)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = _userManager.GetUserAsync(User!).Result;
        ViewData["UserName"] = user?.FullName;

        return View();
    }
}

