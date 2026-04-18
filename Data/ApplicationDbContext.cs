using Microsoft.EntityFrameworkCore;
using BrownieShop.API.Models;

namespace BrownieShop.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Brownie> Brownies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Seed some default brownies
            modelBuilder.Entity<Brownie>().HasData(
                new Brownie { Id = 1, Name = "Classic Brownie", Description = "A rich, moist chocolate brownie.", Price = 120, ImageUrl = "classic.jpg" },
                new Brownie { Id = 2, Name = "Nutella Brownie", Description = "Loaded with creamy Nutella.", Price = 180, ImageUrl = "nutella.jpg" },
                new Brownie { Id = 3, Name = "Walnut Brownie", Description = "Classic chocolate brownie with crunchy walnuts.", Price = 150, ImageUrl = "walnut.jpg" }
            );
        }
    }
}
