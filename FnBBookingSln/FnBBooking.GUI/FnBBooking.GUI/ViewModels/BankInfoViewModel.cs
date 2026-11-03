using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FnBBooking.GUI.ViewModels
{
    public class BankInfoViewModel : BaseMasterDataViewModel
    {
        [Required(ErrorMessage = "Tên ngân hàng không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên ngân hàng không được vượt quá 100 ký tự")]
        public string BankName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên chủ tài khoản không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên chủ tài khoản không được vượt quá 100 ký tự")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số tài khoản không được để trống")]
        [MaxLength(50, ErrorMessage = "Số tài khoản không được vượt quá 50 ký tự")]
        public string AccountNo { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
        public string? TaxCode { get; set; }

        [MaxLength(50, ErrorMessage = "Loại tổ chức không được vượt quá 50 ký tự")]
        public string? OrgType { get; set; } = "Nội bộ";

        public BankInfoViewModel Clone()
        {
            return new BankInfoViewModel
            {
                Id = this.Id,
                Number = this.Number,
                Name = this.Name,
                Description = this.Description,
                BankName = this.BankName,
                AccountName = this.AccountName,
                AccountNo = this.AccountNo,
                TaxCode = this.TaxCode,
                OrgType = this.OrgType,
                IsActive = this.IsActive,
                IsDeleted = this.IsDeleted,
                CreatedOn = this.CreatedOn,
                CreatedBy = this.CreatedBy,
                ModifiedOn = this.ModifiedOn,
                ModifiedBy = this.ModifiedBy
            };
        }

        public static List<BankInfoViewModel> CreateSampleList()
        {
            return new List<BankInfoViewModel>
            {
                new BankInfoViewModel
                {
                    Id = 1,
                    Number = "BANK-VCB-01",
                    Name = "Tài khoản Thu Cọc Madame Lân - Vietcombank",
                    BankName = "Vietcombank (VCB) - Chi nhánh Đà Nẵng",
                    AccountName = "CÔNG TY TNHH MADAME LÂN",
                    AccountNo = "0041000888999",
                    TaxCode = "0401234567",
                    OrgType = "Nội bộ",
                    Description = "Tài khoản nhận tiền cọc booking chính của nhà hàng.",
                    IsActive = true,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-60),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-10)
                },
                new BankInfoViewModel
                {
                    Id = 2,
                    Number = "BANK-TCB-02",
                    Name = "Tài khoản Chi Hoa Hồng - Techcombank",
                    BankName = "Techcombank (TCB) - Chi nhánh Sông Hàn",
                    AccountName = "CÔNG TY TNHH MADAME LÂN",
                    AccountNo = "19036788899011",
                    TaxCode = "0401234567",
                    OrgType = "Nội bộ",
                    Description = "Tài khoản chuyên dùng để thanh toán hoa hồng cho HDV và lữ hành.",
                    IsActive = true,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-45),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-5)
                },
                new BankInfoViewModel
                {
                    Id = 3,
                    Number = "BANK-BIDV-03",
                    Name = "Tài khoản Thu Ngân Sân Vườn - BIDV",
                    BankName = "BIDV - Chi nhánh Hải Châu",
                    AccountName = "CÔNG TY TNHH MADAME LÂN",
                    AccountNo = "56110000876543",
                    TaxCode = "0401234567",
                    OrgType = "Nội bộ",
                    Description = "Tài khoản phụ dự phòng thanh toán trực tuyến qua QR Code.",
                    IsActive = true,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-30),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-2)
                },
                new BankInfoViewModel
                {
                    Id = 4,
                    Number = "BANK-MB-04",
                    Name = "Tài khoản MB Bank - Madame Lân",
                    BankName = "MB Bank - Chi nhánh Đà Nẵng",
                    AccountName = "CÔNG TY TNHH MADAME LÂN",
                    AccountNo = "0888999777",
                    TaxCode = "0401234567",
                    OrgType = "Nội bộ",
                    Description = "Tài khoản số đẹp MB Bank phục vụ chuyển khoản nhanh Napas 24/7.",
                    IsActive = true,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-20),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-1)
                },
                new BankInfoViewModel
                {
                    Id = 5,
                    Number = "BANK-ACB-05",
                    Name = "Tài khoản ACB (Tạm ngừng)",
                    BankName = "ACB - Chi nhánh Bạch Đằng",
                    AccountName = "CÔNG TY TNHH MADAME LÂN",
                    AccountNo = "2468101214",
                    TaxCode = "0401234567",
                    OrgType = "Nội bộ",
                    Description = "Tài khoản cũ đang tạm ngừng giao dịch để đối soát.",
                    IsActive = false,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-90),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-25)
                }
            };
        }
    }
}
