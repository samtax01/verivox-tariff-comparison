using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TariffComparisonApp.Models
{
    public record ProductPriceCalculation: BaseModel
    {
        
        public Guid ProductId { get; set; }
        
        //public Product Product { get; set; }
        
        public int Base { get; set; }
        
        public int? Limit { get; set; }

        /// <summary>
        /// Consumption cost per Kwh
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerKwh { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        public decimal GetAnnualCosts(int value)
        {
            var fixedCost = Base * Amount;
            value = value - Limit ?? value;
            return value < 0 ? 
                fixedCost : 
                fixedCost + value * PricePerKwh;
        }

    }

}