using System;
using System.Threading.Tasks;
using FluentValidation;
using TariffComparisonApp.Models.Requests;
using TariffComparisonApp.Validations;


namespace TariffComparisonApp.Helpers
{
    /// <summary>
    /// A Simple Wrapper for FluentValidation
    /// </summary>
    public static class FluentValidation
    {
        
        
        public static async Task ValidateConsumptionRequest(ConsumptionRequest request)
        {
            var validator =  await new ConsumptionRequestValidation().ValidateAsync(request);
            if (!validator.IsValid)
                throw new ArgumentException(validator.ToString());   
        }
        
        
    }
}