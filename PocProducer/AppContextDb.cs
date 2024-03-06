using Microsoft.EntityFrameworkCore;
using PocProducer.Models;

namespace PocProducer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}