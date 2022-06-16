using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
namespace Maistanesys.Models
{
    public class OrdersContext : DbContext
    {
        public OrdersContext(DbContextOptions<OrdersContext> options)
                : base(options)
        {
        }

        public DbSet<Order>? Carts { get; set; }
        public DbSet<User>? Users { get; set; }

        public DbSet<Item>? Items { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; }
        public DbSet<Courrier>? Courriers { get; set; }

        public DbSet<Order>? Orders { get; set; }


    }
}
