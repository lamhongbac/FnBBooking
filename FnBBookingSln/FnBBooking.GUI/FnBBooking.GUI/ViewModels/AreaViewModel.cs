using System;
using System.Collections.Generic;

namespace FnBBooking.GUI.ViewModels
{
    public class AreaViewModel : BaseMasterDataViewModel
    {
        public int TableCount { get; set; } = 0;

        public AreaViewModel Clone()
        {
            return new AreaViewModel
            {
                Id = this.Id,
                Number = this.Number,
                Name = this.Name,
                Description = this.Description,
                IsActive = this.IsActive,
                IsDeleted = this.IsDeleted,
                CreatedOn = this.CreatedOn,
                CreatedBy = this.CreatedBy,
                ModifiedOn = this.ModifiedOn,
                ModifiedBy = this.ModifiedBy,
                TableCount = this.TableCount
            };
        }

        public static List<AreaViewModel> CreateSampleList()
        {
            return new List<AreaViewModel>
            {
                new AreaViewModel
                {
                    Id = 1,
                    Number = "KV-SANVUON",
                    Name = "Khu Vực Sân Vườn",
                    Description = "Khu vực sân vườn thoáng mát ngoài trời, cây xanh bao quanh, phù hợp đoàn đông.",
                    IsActive = true,
                    TableCount = 12,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-30),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-5)
                },
                new AreaViewModel
                {
                    Id = 2,
                    Number = "KV-PHOCO-T1",
                    Name = "Tầng 1 - Không Gian Phố Cổ",
                    Description = "Không gian tái hiện kiến trúc phố cổ Hội An với đèn lồng và gạch cổ.",
                    IsActive = true,
                    TableCount = 16,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-28),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-2)
                },
                new AreaViewModel
                {
                    Id = 3,
                    Number = "KV-HOADANG-T2",
                    Name = "Tầng 2 - Sảnh Hoa Đăng",
                    Description = "Sảnh tầng 2 view rộng, phong cách ấm cúng, phù hợp tiệc gia đình và liên hoan.",
                    IsActive = true,
                    TableCount = 10,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-25),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-1)
                },
                new AreaViewModel
                {
                    Id = 4,
                    Number = "KV-VIP-HOIAN1",
                    Name = "Phòng VIP Hội An 1",
                    Description = "Phòng VIP riêng tư có điều hòa, sức chứa tối đa 16 khách, phục vụ khách đoàn cao cấp.",
                    IsActive = true,
                    TableCount = 2,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-20),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-10)
                },
                new AreaViewModel
                {
                    Id = 5,
                    Number = "KV-VENSONG",
                    Name = "Khu Vực Ven Sông",
                    Description = "Bàn ăn dọc bờ sông lãng mạn, view ngắm cảnh về đêm.",
                    IsActive = true,
                    TableCount = 8,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Today.AddDays(-15),
                    ModifiedBy = "admin",
                    ModifiedOn = DateTime.Today.AddDays(-3)
                }
            };
        }
    }
}

