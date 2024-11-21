using Microsoft.EntityFrameworkCore;
using PaymentProvider.Entities;

namespace PaymentProvider.Contexts
{
    public class RikaOrdersDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<InvoiceEntity> Invoices { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ShippingEntity> ShippingDetails { get; set; }
    }
}
