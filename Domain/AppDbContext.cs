using Microsoft.EntityFrameworkCore;
using WebServers.Domain.Models;

namespace WebServers.Domain
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<VirtualServer> VirtualServers { get; set; }
    }
}
