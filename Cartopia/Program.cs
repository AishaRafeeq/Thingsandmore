using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cartopia.Data;
using Cartopia.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure the database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity with ApplicationUser and Role-based access control
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6; // Minimum password length
    options.SignIn.RequireConfirmedAccount = false; // Set to true if email confirmation is required
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookies for authentication/authorization
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redirects unauthenticated users to Login
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redirects unauthorized users
});

// Configure session management (for features like cart handling)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout duration
    options.Cookie.HttpOnly = true; // Restrict cookie access to HTTP
    options.Cookie.IsEssential = true; // GDPR compliance
});

// Add controllers with views
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); // Useful for development errors

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint(); // Endpoint for database migrations during development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Custom error handling in production
    app.UseHsts(); // Enforce HTTPS
}



app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
app.UseStaticFiles();       // Serve static files (e.g., CSS, JS)
app.UseRouting();           // Enable routing

app.UseAuthentication(); // User authentication
app.UseAuthorization();  // Access control
app.UseSession();        // Enable session management

// Seed roles and default admin user during startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedRoleAndAdminAsync(services); // Call the role/user seeding method
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during role and user seeding.");
    }
}

// Map routes for controllers and Razor Pages
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapRazorPages(); // Allows Razor Pages to work alongside MVC

app.Run();

// -------------------- Role/User Seeding Logic ----------------------

static async Task SeedRoleAndAdminAsync(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Create default admin user if it doesn't exist
    var adminEmail = "admin@cartopia.com";
    var adminPassword = "Admin@123"; // Ensure this matches your password policy

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "Default Admin",
            Address = "123 Admin Street",
            City = "Admin City",
            Country = "Admin Country"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
