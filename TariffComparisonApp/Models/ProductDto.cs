using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TariffComparisonApp.Models
{
    public record ProductDto
    {
        
        [MaxLength(50)]
        public string TariffName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AnnualCosts { get; set; }
        
    }

}