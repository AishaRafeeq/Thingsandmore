using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cartopia.Models;
using Microsoft.AspNetCore.Identity;

namespace Cartopia.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for application models
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize table names for Identity
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

            // Configure decimal properties to prevent truncation
            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18, 2)");
            builder.Entity<OrderDetail>().Property(od => od.Price).HasColumnType("decimal(18, 2)");
            builder.Entity<Order>().Property(o => o.TotalAmount).HasColumnType("decimal(18, 2)");

            // Relationships and additional configurations
            // Order -> OrderDetails (One-to-Many)
            builder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderDetail -> Product (Many-to-One)
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // ShoppingCartItem -> Product (Many-to-One)
            builder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Product)
                .WithMany()
                .HasForeignKey(sci => sci.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            // Default value for IsFeatured in Product
            builder.Entity<Product>()
                .Property(p => p.IsFeatured)
                .HasDefaultValue(false); // Default value for the new IsFeatured column
        }
    }
}