using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TariffComparisonApp.Data;
using TariffComparisonApp.Tests.Helpers;
using Xunit.Abstractions;

namespace TariffComparisonApp.Tests
{
    public abstract class DiResolver: AbstractIntegrationTest {

        protected readonly ITestOutputHelper TestOutputHelper;

        protected readonly IConfigurationRoot ConfigurationRoot;
        
        public DiResolver(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
            
            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json") // Dev must be last. e.g in Queue. So we don't affect production queue
                .Build();
        }

        
        
        /// <summary>
        /// Seed Data
        /// </summary>
        protected async Task SeedDataAsync(DatabaseContext databaseContextSimulator = null)
        {
            
            databaseContextSimulator ??= DatabaseContext;
            if (await databaseContextSimulator.Products.AnyAsync())
                return;

            await databaseContextSimulator.Database.EnsureCreatedAsync();

            var products = Seeders.GetSampleProducts();
            
            await databaseContextSimulator.Products.AddRangeAsync(products);
            await databaseContextSimulator.SaveChangesAsync();
        }
    
        
    }
}