using System.ComponentModel.DataAnnotations;

namespace TariffComparisonApp.Models
{
    public record Product: BaseModel
    {
        
        [MaxLength(50)]
        public string TariffName { get; set; }

        public ProductPriceCalculation PriceCalculation { get; set; }
    }

}