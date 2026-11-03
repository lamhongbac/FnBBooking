using System.ComponentModel.DataAnnotations;

namespace FnBBooking.GUI.ViewModels
{
    public class BaseMasterDataViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mã (Number) không được để trống")]
        [MaxLength(20, ErrorMessage = "Mã không được vượt quá 20 ký tự")]
        public string Number { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên (Name) không được để trống")]
        [MaxLength(250, ErrorMessage = "Tên không được vượt quá 250 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}

