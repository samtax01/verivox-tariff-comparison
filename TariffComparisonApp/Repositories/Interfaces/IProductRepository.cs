using System.Collections.Generic;
using System.Threading.Tasks;
using TariffComparisonApp.Models;
using TariffComparisonApp.Models.Requests;

namespace TariffComparisonApp.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDto>> CompareCostsAsync(ConsumptionRequest consumptionRequest);
    }
}