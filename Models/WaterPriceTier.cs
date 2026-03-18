using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class WaterPriceTier
    {
        public int TierId { get; set; }

        [Required]
        public int TierLevel { get; set; }

        [Required]
        public decimal FromM3 { get; set; }

        [Required]
        public decimal ToM3 { get; set; }

        [Required]
        public decimal PricePerM3 { get; set; }

        [Required]
        public DateTime EffectiveDate { get; set; }

        [Required]
        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
