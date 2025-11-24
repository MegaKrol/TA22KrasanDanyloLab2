using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TA22KrasanLab2.Models;
using Microsoft.EntityFrameworkCore;

namespace TA22KrasanLab2.Data
{
    public class FarmContext : IdentityDbContext
    {
        public FarmContext(DbContextOptions<FarmContext> options)
            : base(options)
        {
        }
        public DbSet<Pig> Pigs { get; set; } = null;
        public DbSet<Farmhouse> Farmhouses { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
