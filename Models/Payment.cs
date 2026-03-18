using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public int BillId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "CASH";

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? PaymentRefCode { get; set; }

        [StringLength(255)]
        public string? Notes { get; set; }

        public int? ConfirmedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public WaterBill? WaterBill { get; set; }
        public Customer? Customer { get; set; }
        public UserAccount? ConfirmedByUser { get; set; }
    }
}
