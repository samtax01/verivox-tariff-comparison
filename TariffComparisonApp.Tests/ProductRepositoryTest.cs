using System.Linq;
using System.Threading.Tasks;
using TariffComparisonApp.Models.Requests;
using TariffComparisonApp.Repositories;
using TariffComparisonApp.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace TariffComparisonApp.Tests
{
    public class ProductRepositoryTest: DiResolver
    {
        public ProductRepositoryTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }
        
        
        [Fact]
        public async Task CompareTest()
        {
            // Arrange
            await SeedDataAsync(DatabaseContext);
            var repository = new ProductRepository(DatabaseContext);
            
            // Assert
            var result = await repository.CompareCostsAsync(new ConsumptionRequest{Consumption = 3500});

            // Assert
            Assert.Equal(2, result.Count());
            Assert.True(result.First().AnnualCosts <= result.Last().AnnualCosts);
        }


    }
}