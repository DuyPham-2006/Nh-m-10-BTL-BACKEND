using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class DeceasedPerson
    {
        public int DeceasedId { get; set; }

        [Required(ErrorMessage = "Tên người mất là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = null!;    // Tên người mất

        [Required(ErrorMessage = "Năm sinh là bắt buộc")]
        [Range(1900, 2100, ErrorMessage = "Năm sinh phải từ 1900 đến 2100")]
        public int BirthYear { get; set; }            // Năm sinh

        [Required(ErrorMessage = "Năm mất là bắt buộc")]
        [Range(1900, 2100, ErrorMessage = "Năm mất phải từ 1900 đến 2100")]
        public int DeathYear { get; set; }            // Năm mất

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }      // Mô tả thêm

        public DateTime CreatedDate { get; set; } = new DateTime(2026, 3, 17);

// Khóa ngoại
        public int? GraveId { get; set; }

// Quan hệ
        public Grave? Grave { get; set; }
        public ICollection<Relative> Relatives { get; set; } = new List<Relative>();
    }
}
