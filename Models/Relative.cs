using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class Relative
    {
        public int RelativeId { get; set; }

        [Required(ErrorMessage = "Tên thân nhân là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; } = null!;     // Tên thân nhân

        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string? PhoneNumber { get; set; }          // Số điện thoại

        [StringLength(50, ErrorMessage = "Quan hệ không được vượt quá 50 ký tự")]
        public string? Relationship { get; set; }         // Quan hệ (Con, Vợ, Chồng, Bố, Mẹ, ...)

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string? Email { get; set; }                // Email

        [StringLength(300, ErrorMessage = "Địa chỉ không được vượt quá 300 ký tự")]
        public string? Address { get; set; }              // Địa chỉ

        public DateTime CreatedDate { get; set; } = new DateTime(2026, 3, 17);

// Khóa ngoại
        [Required(ErrorMessage = "ID người mất là bắt buộc")]
        public int DeceasedId { get; set; }

// Quan hệ
        public DeceasedPerson? DeceasedPerson { get; set; }
    }
}
