using System.Collections.Generic;
using TariffComparisonApp.Models;

namespace TariffComparisonApp.Tests
{
    public class Seeders
    {
        
        public const string Product1Name = "Basic electricity tariff";
        
        public const string Product2Name = "Packaged tariff";
        
        
        public static IEnumerable<Product> GetSampleProducts()
        {
            var product1 = new Product
            {
                TariffName = Product1Name,
                PriceCalculation = new ProductPriceCalculation
                {
                    Amount = 5,
                    Base = 12,
                    PricePerKwh = 0.22m
                }
            };
            
            var product2 = new Product
            {
                TariffName = Product2Name,
                PriceCalculation = new ProductPriceCalculation
                {
                    Amount = 800,
                    Base = 1,
                    Limit = 4000,
                    PricePerKwh = 0.30m
                }
            };

            return new[] {product1, product2};
        }
        
    }
}