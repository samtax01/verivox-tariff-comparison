using Microsoft.EntityFrameworkCore;
using TariffApp.Models;

namespace TariffApp.Data
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions): base(dbContextOptions) { }
        
        public DbSet<Product> Products { get; set;  }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity => entity.HasIndex(p => new {p.AnnualCosts} ));
        }
        
    }
}