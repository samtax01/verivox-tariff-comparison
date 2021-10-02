using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TariffComparisonApp.Models
{
    public record Product: BaseModel
    {
        
        [MaxLength(50)]
        public string TariffName { get; set; }

        public ProductPriceCalculation PriceCalculation { get; set; }
    }

}