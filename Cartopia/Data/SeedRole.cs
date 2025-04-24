namespace Cartopia.Data
{
    using global::Cartopia.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    namespace Cartopia.Data
    {
        public static class SeedRole
        {
            public static async Task Initialize(IServiceProvider serviceProvider)
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                
                string[] roles = { "Admin", "Customer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                
                var adminEmail = "admin@cartopia.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var newAdmin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "Default Admin",
                        Address = "Cartopia HQ",
                        City = "Admin City",
                        Country = "Admin Country",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(newAdmin, "Admin@123"); 
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin, "Admin");
                    }
                    else
                    {
                        throw new Exception("Failed to create default admin user.");
                    }
                }
            }
        }
    }
}
