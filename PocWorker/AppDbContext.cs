using Microsoft.EntityFrameworkCore;
using PocWorker.Models;

namespace PocWorker
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}