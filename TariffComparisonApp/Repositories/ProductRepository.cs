using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TariffComparisonApp.Data;
using TariffComparisonApp.Models;
using TariffComparisonApp.Models.Requests;
using TariffComparisonApp.Repositories.Interfaces;

namespace TariffComparisonApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProductRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
  
        public async Task<IEnumerable<ProductDto>> CompareCostsAsync(ConsumptionRequest consumptionRequest)
        {
            // TODO: seeding demo products(if not exists) for testing purposes.
            await SeedDatabaseWithSampleProducts();
            
            // Compare products
            return _databaseContext.Products.Include(x=>x.PriceCalculation).AsEnumerable().Select(p=>new ProductDto
            {
                TariffName = p.TariffName,
                AnnualCosts = p.PriceCalculation.GetAnnualCosts(consumptionRequest.Consumption)
            }).OrderBy(p=>p.AnnualCosts);
        }
        
        
        
        
        /// <summary>
        /// TODO: Sample products seeder method for testing [to be deleted]
        /// </summary>
        public async Task SeedDatabaseWithSampleProducts()
        {
            
            if (await _databaseContext.Products.AnyAsync())
                return;
            
            var products = new[]
            {
                new Product
                {
                    TariffName = "Basic electricity tariff",
                    PriceCalculation = new ProductPriceCalculation
                    {
                        Amount = 5,
                        Base = 12,
                        PricePerKwh = 0.22m
                    }
                },

                new Product
                {
                    TariffName = "Packaged tariff",
                    PriceCalculation = new ProductPriceCalculation
                    {
                        Amount = 800,
                        Base = 1,
                        Limit = 4000,
                        PricePerKwh = 0.30m
                    }
                }
            };
            await _databaseContext.Products.AddRangeAsync(products);
            if (await _databaseContext.SaveChangesAsync() <= 0)
                throw new Exception(ConfigurationData.Locale.FailedToSave);
        }
        
        
    }
}