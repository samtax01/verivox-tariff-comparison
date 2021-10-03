using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ehex.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TariffComparisonApp.Data;
using TariffComparisonApp.Models;
using TariffComparisonApp.Models.Requests;
using TariffComparisonApp.Repositories.Interfaces;
using static TariffComparisonApp.Helpers.FluentValidation;

namespace TariffComparisonApp.Controllers
{
    [ApiController]
    [Route(ConfigurationData.ApiVersion + "/tariff/products")]
    [Produces("application/json")]
    public class ProductController : Controller
    {
        
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductDto>>), 200)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("compareCosts")]
        [HttpGet]
        public async Task<IActionResult> CompareCostsAsync([FromQuery] ConsumptionRequest consumptionRequest)
        {
            await ValidateConsumptionRequest(consumptionRequest);
            var costsList = await _productRepository.CompareCostsAsync(consumptionRequest);
            return ApiResponse<IEnumerable<ProductDto>>.Success(costsList);
        }
        
        
    }
    
}