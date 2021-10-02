using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TariffComparisonApp.Models;
using Xunit;
using Xunit.Abstractions;

namespace TariffComparisonApp.Tests
{
    public class TariffComparisonTest: DiResolver
    {
        public TariffComparisonTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }
        
        
        [Fact]
        public async Task CompareTest()
        {
            // Arrange
            await SeedDataAsync();
            var sss = await DatabaseContext.Products.FirstOrDefaultAsync();

            // Act
            var products = DatabaseContext.Products.AsEnumerable().Select(p=>new ProductDto
            {
                TariffName = p.TariffName,
                AnnualCosts = p.PriceCalculation.GetAnnualCosts(3500),
            }).OrderBy(p=>p.AnnualCosts);

            // Assert
            Assert.Equal(2, products.Count());
            Assert.True(products.First().AnnualCosts <= products.Last().AnnualCosts);
        }

        
        
        [Fact]
        public async Task Validate_Order_By_Product1_6000()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var products = DatabaseContext.Products.AsEnumerable().Select(p=>new ProductDto
            {
                TariffName = p.TariffName,
                AnnualCosts = p.PriceCalculation.GetAnnualCosts(6000),
            }).OrderBy(p=>p.AnnualCosts);

            // Assert
            Assert.Equal(Seeders.Product1Name, products.First().TariffName);
        } 

        

        [Fact]
        public async Task Validate_Order_By_Product2_3500()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var products = DatabaseContext.Products.AsEnumerable().Select(p=>new ProductDto
            {
                TariffName = p.TariffName,
                AnnualCosts = p.PriceCalculation.GetAnnualCosts(3500),
            }).OrderBy(p=>p.AnnualCosts);

            // Assert
            Assert.Equal(Seeders.Product2Name, products.First().TariffName);
        } 
        

        [Fact]
        public async Task Validate_Order_By_Product2_4500()
        {
            // Arrange
            await SeedDataAsync();

            // Act
            var products = DatabaseContext.Products.AsEnumerable().Select(p=>new ProductDto
            {
                TariffName = p.TariffName,
                AnnualCosts = p.PriceCalculation.GetAnnualCosts(4500),
            }).OrderBy(p=>p.AnnualCosts);

            // Assert
            Assert.Equal(Seeders.Product2Name, products.First().TariffName);
        } 







    }
}