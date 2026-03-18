using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class WaterMeter
    {
        public int MeterId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required, StringLength(50)]
        public string MeterCode { get; set; } = null!;

        [StringLength(50)]
        public string? Brand { get; set; }

        [StringLength(50)]
        public string? Model { get; set; }

        public DateTime InstallDate { get; set; } = DateTime.UtcNow;

        public decimal InitialIndex { get; set; } = 0;

        [Required]
        public string Status { get; set; } = "ACTIVE";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Customer? Customer { get; set; }
        public ICollection<WaterReading> WaterReadings { get; set; } = new List<WaterReading>();
    }
}
