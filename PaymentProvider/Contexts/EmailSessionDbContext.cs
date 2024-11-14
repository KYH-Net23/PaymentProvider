using Microsoft.EntityFrameworkCore;
using PaymentProvider.Models;

namespace PaymentProvider.Contexts
{
    public class EmailSessionDbContext(DbContextOptions<EmailSessionDbContext> options) : DbContext(options)
    {
        public virtual DbSet<EmailSession> Sessions { get; set; }
    }
}
