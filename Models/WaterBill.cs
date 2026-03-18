using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class WaterBill
    {
        public int BillId { get; set; }

        [Required]
        public int ReadingId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int MeterId { get; set; }

        [Required]
        public DateTime BillingMonth { get; set; }

        [Required]
        public decimal TotalConsumptionM3 { get; set; }

        [Required]
        public decimal SubtotalAmount { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal EnvironmentalFee { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string BillStatus { get; set; } = "UNPAID";

        public DateTime BillDate { get; set; } = DateTime.UtcNow;

        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(15);

        public int? ProcessedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public WaterReading? WaterReading { get; set; }
        public Customer? Customer { get; set; }
        public WaterMeter? WaterMeter { get; set; }
        public UserAccount? ProcessedByUser { get; set; }
    }
}
