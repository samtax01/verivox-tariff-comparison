using System.Collections.Generic;
using System.Threading.Tasks;
using Ehex.Helpers;
using TariffComparisonApp.Models;
using TariffComparisonApp.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;
using HttpMethod = System.Net.Http.HttpMethod;

namespace TariffComparisonApp.Tests.Controllers
{
    public class ProductControllerTest: DiResolver
    {
        public ProductControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }
        
        
        [Fact]
        public async Task CompareTest()
        {
            // Act
            var httpResponse = await HttpRequest.MakeAsync(TestServer, "/api/v1/tariff/products/compareCosts?Consumption=3500", HttpMethod.Get);
            var apiResponse = await ApiResponse<IEnumerable<ProductDto>>.FromRequestAsync(httpResponse);
            TestOutputHelper.WriteLine("Response content: {0}", apiResponse);

            // Assert
            Assert.True(apiResponse.Status);
            Assert.NotEmpty(apiResponse.Data);
        }


    }
}