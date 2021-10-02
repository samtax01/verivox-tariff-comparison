using Microsoft.EntityFrameworkCore;
using TariffComparisonApp.Models;

namespace TariffComparisonApp.Data
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions): base(dbContextOptions) { }
        
        public DbSet<Product> Products { get; set;  }
        
        public DbSet<ProductPriceCalculation> ProductPriceCalculations { get; set;  }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
        
    }
}