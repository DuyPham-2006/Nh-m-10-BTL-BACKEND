using System.ComponentModel.DataAnnotations;

namespace PostManagementApp.Models
{
    public class Grave
    {
        public int GraveId { get; set; }

        [Required(ErrorMessage = "Khu vực là bắt buộc")]
        [StringLength(50, ErrorMessage = "Khu vực không được vượt quá 50 ký tự")]
        public string Area { get; set; } = null!;  // Khu vực (A, B, C, ...)

        [Required(ErrorMessage = "Số hàng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số hàng phải lớn hơn 0")]
        public int RowNumber { get; set; }         // Hàng

        [Required(ErrorMessage = "Số mộ là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số mộ phải lớn hơn 0")]
        public int GraveNumber { get; set; }       // Số mộ

        [StringLength(500, ErrorMessage = "Vị trí không được vượt quá 500 ký tự")]
        public string? Location { get; set; }      // Vị trí chi tiết

        public DateTime CreatedDate { get; set; } = new DateTime(2026, 3, 17);

        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
public string? Status { get; set; } = "Available"; // Trống, Đã sử dụng, Đặt trước

// Quan hệ: Một mộ có Một người mất
        public DeceasedPerson? DeceasedPerson { get; set; }
    }
}

