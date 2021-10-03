using System;
using FluentValidation;
using TariffComparisonApp.Models.Requests;

namespace TariffComparisonApp.Validations
{
    public class ConsumptionRequestValidation: AbstractValidator<ConsumptionRequest>
    {
        public ConsumptionRequestValidation()
        {
            
            RuleFor(x => x.Consumption)
                .NotEmpty()
                .WithMessage("Please input the Consumption value(kWh/year)");
            
        }
    }
}