using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace RepositooSample
{
    public class OrdersContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        public OrdersContext(DbContextOptions<OrdersContext> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Order>()
                .HasMany(o => o.OrderDetails);
                
            
        }
    }
}
