using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; } = null!;

        [Required, StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [Required, StringLength(100)]
        public string FullName { get; set; } = null!;

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; } = "STAFF";

        [Required]
        public string Status { get; set; } = "ACTIVE";

        public DateTime? LastLogin { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }

        public ICollection<WaterReading> WaterReadings { get; set; } = new List<WaterReading>();
        public ICollection<WaterBill> ProcessedBills { get; set; } = new List<WaterBill>();
        public ICollection<WaterSupply> RecordedSupplies { get; set; } = new List<WaterSupply>();
        public ICollection<Payment> ConfirmedPayments { get; set; } = new List<Payment>();
    }
}
