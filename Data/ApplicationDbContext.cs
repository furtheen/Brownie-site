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
                new Brownie { Id = 1, Name = "Classic Brownie", Description = "Rich dark chocolate, dense and fudgy. The one that started it all.", Price = 60, ImageUrl = "🍫" },
                new Brownie { Id = 2, Name = "Walnut Brownie", Description = "Classic brownie loaded with crunchy California walnuts on every bite.", Price = 75, ImageUrl = "🌰" },
                new Brownie { Id = 3, Name = "Choco Chip Brownie", Description = "Double chocolate — fudgy base + melted choco chips throughout.", Price = 70, ImageUrl = "🍪" },
                new Brownie { Id = 4, Name = "Oreo Brownie", Description = "Crushed Oreo cookies baked right into our signature brownie batter.", Price = 80, ImageUrl = "⚫" },
                new Brownie { Id = 5, Name = "Nutella Brownie", Description = "A swirl of Nutella baked into every bite. Pure indulgence.", Price = 90, ImageUrl = "🫙" },
                new Brownie { Id = 6, Name = "Brownie Sundae", Description = "Warm brownie topped with vanilla ice cream and chocolate drizzle.", Price = 120, ImageUrl = "🍨" }
            );
        }
    }
}
