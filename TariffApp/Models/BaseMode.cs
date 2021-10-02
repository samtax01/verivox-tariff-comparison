using System;

namespace TariffApp.Models
{
    public abstract record BaseModel
    {
        /// <summary>
        /// CreatedAt. Using ISO-8601 standard
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// UpdatedAt. Using ISO-8601 standard
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        
        public BaseModel()
        {
            CreatedAt = DateTime.UtcNow; 
            UpdatedAt = DateTime.UtcNow;
        }
        
    }

}