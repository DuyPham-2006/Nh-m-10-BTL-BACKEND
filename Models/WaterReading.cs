using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class WaterReading
    {
        public int ReadingId { get; set; }

        [Required]
        public int MeterId { get; set; }

        [Required]
        public DateTime ReadingMonth { get; set; }

        [Required]
        public decimal OldIndex { get; set; }

        [Required]
        public decimal NewIndex { get; set; }

        public decimal ConsumptionM3 => NewIndex - OldIndex;

        public int? ReadingStaffId { get; set; }

        public DateTime ReadingDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Status { get; set; } = "PENDING";

        [StringLength(255)]
        public string? Notes { get; set; }

        public WaterMeter? WaterMeter { get; set; }
        public UserAccount? ReadingStaff { get; set; }
        public WaterBill? WaterBill { get; set; }
    }
}
