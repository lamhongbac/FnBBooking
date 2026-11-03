using System.ComponentModel.DataAnnotations;

namespace FnBBooking.GUI.ViewModels
{
    public class ContactTypeViewModel : BaseMasterDataViewModel
    {
        [Display(Name = "Có hoa hồng")]
        public bool HaveCommision { get; set; }

        [Display(Name = "Tỷ lệ hoa hồng Set Menu (%)")]
        [Range(0, 100, ErrorMessage = "Tỷ lệ hoa hồng phải từ 0% đến 100%")]
        public decimal CommissionRate1 { get; set; }

        [Display(Name = "Tỷ lệ hoa hồng A-la-carte (%)")]
        [Range(0, 100, ErrorMessage = "Tỷ lệ hoa hồng phải từ 0% đến 100%")]
        public decimal CommissionRate2 { get; set; }
    }
}

