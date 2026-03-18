using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class DeceasedFullInfo
    {
        public int DeceasedId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int BirthYear { get; set; }
        public int DeathYear { get; set; }
        public string? Description { get; set; }

        // Grave Information
        public string? GraveArea { get; set; }
        public int? GraveRowNumber { get; set; }
        public int? GraveNumber { get; set; }
        public string? GraveLocation { get; set; }

        // Relatives Information
        public List<RelativeInfo> Relatives { get; set; } = new List<RelativeInfo>();
    }

    public class RelativeInfo
    {
        public int RelativeId { get; set; }
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Relationship { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}