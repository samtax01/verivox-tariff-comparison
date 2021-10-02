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

        





    }
}