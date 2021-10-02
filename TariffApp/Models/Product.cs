namespace TariffApp.Models
{
    public record Product: BaseModel
    {
        
        public string TariffName { get; set; }
        
        public decimal AnnualCosts { get; set; }
        
    }

}