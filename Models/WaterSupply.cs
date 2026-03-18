using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class WaterSupply
    {
        public int SupplyId { get; set; }

        [Required]
        public DateTime SupplyMonth { get; set; }

        [Required]
        public decimal TotalM3Pumped { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        public int? RecordedBy { get; set; }

        public DateTime RecordedDate { get; set; } = DateTime.UtcNow;

        public UserAccount? RecordedByUser { get; set; }
    }
}
