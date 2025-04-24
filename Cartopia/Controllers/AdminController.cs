using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Cartopia.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Cartopia.Controllers
{


    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ResetTestData()
        {
            TempData.Clear();
            HttpContext.Session.Clear();

         
            return RedirectToAction("Dashboard", "Admin");
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
       
            var model = new AdminDashboardViewModel
            {
                TotalUsers = 150,
                TotalRoles = 5,
                TotalOrders = 45,
                RecentUsers = new List<string> { "John Doe", "Jane Smith", "Alice Johnson" },
                RecentRoles = new List<string> { "Admin", "Editor", "Viewer" },
                RecentOrders = new List<AdminDashboardViewModel.OrderViewModel>
            {
                new AdminDashboardViewModel.OrderViewModel { OrderID = 101, TotalAmount = 500.00M },
                new AdminDashboardViewModel.OrderViewModel { OrderID = 102, TotalAmount = 1500.00M }
            }
            };

            return View(model); 
        }

    
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            Debug.WriteLine($"Total Users: {users.Count}");

            var userViewModels = new List<ManageUserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Debug.WriteLine($"User: {user.FullName}, Roles: {string.Join(", ", roles)}");

                userViewModels.Add(new ManageUserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email!,
                    Roles = roles
                });
            }

            return View(userViewModels);
        }


        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "User ID cannot be null or empty.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = $"User with ID {id} not found.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete user.";
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                TempData["Error"] = "User ID and role cannot be null or empty.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = $"User with ID {userId} not found.";
                return RedirectToAction(nameof(ManageUsers));
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                TempData["Error"] = $"Role {role} does not exist.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role {role} assigned to user {user.FullName} successfully.";
            }
            else
            {
                TempData["Error"] = $"Failed to assign role {role} to user {user.FullName}.";
            }

            return RedirectToAction(nameof(ManageUsers));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                TempData["Error"] = "User ID and role cannot be null or empty.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = $"User with ID {userId} not found.";
                return RedirectToAction(nameof(ManageUsers));
            }

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                TempData["Error"] = $"User {user.FullName} is not assigned to the role {role}.";
                return RedirectToAction(nameof(ManageUsers));
            }

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                TempData["Success"] = $"Role {role} removed from user {user.FullName} successfully.";
            }
            else
            {
                TempData["Error"] = $"Failed to remove role {role} from user {user.FullName}.";
            }

            return RedirectToAction(nameof(ManageUsers));
        }
    }
}
