using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required, StringLength(255)]
        public string Address { get; set; } = null!;

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? MeterCode { get; set; }

        [Required]
        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        public ICollection<WaterMeter> WaterMeters { get; set; } = new List<WaterMeter>();
        public ICollection<WaterBill> WaterBills { get; set; } = new List<WaterBill>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
