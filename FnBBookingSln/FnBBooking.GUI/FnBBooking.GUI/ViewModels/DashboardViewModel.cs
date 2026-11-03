using System;
using System.Collections.Generic;

namespace FnBBooking.GUI.ViewModels
{
    public class DashboardStatItem
    {
        public string Title { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string BadgeText { get; set; } = string.Empty;
        public string BadgeClass { get; set; } = "bg-light-primary text-primary";
        public string IconClass { get; set; } = "ti ti-trending-up";
        public bool IsIncrease { get; set; } = true;
    }

    public class RecentBookingItem
    {
        public string BookingCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public string BookingTime { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string StatusBadgeClass { get; set; } = "badge bg-light-primary text-primary";
        public decimal TotalAmount { get; set; }

        public string GuestSummary => ChildCount > 0 ? $"{AdultCount} Lớn, {ChildCount} Trẻ" : $"{AdultCount} Khách";
    }

    public class DashboardActivityItem
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TimeAgo { get; set; } = string.Empty;
        public string AmountText { get; set; } = string.Empty;
        public string IconClass { get; set; } = "ti ti-calendar";
        public string AvatarBgClass { get; set; } = "bg-light-primary text-primary";
    }

    public class DashboardViewModel : BaseViewModel
    {
        public string RestaurantName { get; set; } = "Madame Lân - Phố Cổ Hội An";
        public string WelcomeMessage { get; set; } = "Tổng quan hoạt động đặt bàn và kinh doanh trong ngày.";
        public DateTime ReportDate { get; set; } = DateTime.Today;

        public List<DashboardStatItem> Stats { get; set; } = new();
        public List<RecentBookingItem> RecentBookings { get; set; } = new();
        public List<DashboardActivityItem> RecentActivities { get; set; } = new();

        public static DashboardViewModel CreateSampleData()
        {
            return new DashboardViewModel
            {
                Stats = new List<DashboardStatItem>
                {
                    new DashboardStatItem
                    {
                        Title = "Booking Hôm Nay",
                        Value = "48 lượt",
                        Subtitle = "So với hôm qua",
                        BadgeText = "+14.5%",
                        BadgeClass = "bg-light-success text-success",
                        IconClass = "ti ti-calendar-event",
                        IsIncrease = true
                    },
                    new DashboardStatItem
                    {
                        Title = "Khách Đang Phục Vụ",
                        Value = "156 khách",
                        Subtitle = "Tỷ lệ lấp đầy bàn",
                        BadgeText = "78%",
                        BadgeClass = "bg-light-primary text-primary",
                        IconClass = "ti ti-users",
                        IsIncrease = true
                    },
                    new DashboardStatItem
                    {
                        Title = "Doanh Thu Dự Kiến",
                        Value = "38,500,000 đ",
                        Subtitle = "Bao gồm cọc trước",
                        BadgeText = "+8.2%",
                        BadgeClass = "bg-light-warning text-warning",
                        IconClass = "ti ti-cash",
                        IsIncrease = true
                    },
                    new DashboardStatItem
                    {
                        Title = "Hoa Hồng Chờ Duyệt",
                        Value = "5 yêu cầu",
                        Subtitle = "Cần quản lý duyệt",
                        BadgeText = "5 pending",
                        BadgeClass = "bg-light-danger text-danger",
                        IconClass = "ti ti-file-invoice",
                        IsIncrease = false
                    }
                },
                RecentBookings = new List<RecentBookingItem>
                {
                    new RecentBookingItem
                    {
                        BookingCode = "BK-261103-01",
                        CustomerName = "Đoàn Du Lịch Saigontourist",
                        PhoneNumber = "0905 123 456",
                        AdultCount = 25,
                        ChildCount = 3,
                        BookingTime = "11:30 - Hôm nay",
                        AreaName = "Khu Vực Sân Vườn",
                        Status = "Đã Check-in",
                        StatusBadgeClass = "badge bg-light-success text-success",
                        TotalAmount = 8500000
                    },
                    new RecentBookingItem
                    {
                        BookingCode = "BK-261103-02",
                        CustomerName = "Gia đình Anh Nguyễn Tuấn",
                        PhoneNumber = "0914 987 654",
                        AdultCount = 6,
                        ChildCount = 2,
                        BookingTime = "12:00 - Hôm nay",
                        AreaName = "Phòng VIP Hội An 1",
                        Status = "Đã Xác Nhận",
                        StatusBadgeClass = "badge bg-light-primary text-primary",
                        TotalAmount = 3200000
                    },
                    new RecentBookingItem
                    {
                        BookingCode = "BK-261103-03",
                        CustomerName = "Công ty TNHH Ánh Dương",
                        PhoneNumber = "0988 555 777",
                        AdultCount = 12,
                        ChildCount = 0,
                        BookingTime = "18:00 - Hôm nay",
                        AreaName = "Tầng 2 - Hoa Đăng",
                        Status = "Chờ Duyệt Cọc",
                        StatusBadgeClass = "badge bg-light-warning text-warning",
                        TotalAmount = 5600000
                    },
                    new RecentBookingItem
                    {
                        BookingCode = "BK-261103-04",
                        CustomerName = "Chị Lê Mai Lan (Khách lẻ)",
                        PhoneNumber = "0935 222 111",
                        AdultCount = 4,
                        ChildCount = 1,
                        BookingTime = "19:00 - Hôm nay",
                        AreaName = "Khu Vực Trong Nhà",
                        Status = "Đã Xác Nhận",
                        StatusBadgeClass = "badge bg-light-primary text-primary",
                        TotalAmount = 1850000
                    },
                    new RecentBookingItem
                    {
                        BookingCode = "BK-261103-05",
                        CustomerName = "HDV Trần Hoàng (Tour Khách Hàn)",
                        PhoneNumber = "0903 888 999",
                        AdultCount = 18,
                        ChildCount = 0,
                        BookingTime = "19:30 - Hôm nay",
                        AreaName = "Khu Vực Ven Sông",
                        Status = "Đã Check-in",
                        StatusBadgeClass = "badge bg-light-success text-success",
                        TotalAmount = 7200000
                    }
                },
                RecentActivities = new List<DashboardActivityItem>
                {
                    new DashboardActivityItem
                    {
                        Title = "Check-in thành công",
                        Description = "Đoàn Saigontourist (28 khách) vào bàn V-01 -> V-03",
                        TimeAgo = "10 phút trước",
                        AmountText = "+ 8,500,000 đ",
                        IconClass = "ti ti-user-check",
                        AvatarBgClass = "bg-light-success text-success"
                    },
                    new DashboardActivityItem
                    {
                        Title = "Nhận đặt cọc mới",
                        Description = "Booking BK-261103-03 chuyển khoản cọc qua Vietcombank",
                        TimeAgo = "35 phút trước",
                        AmountText = "+ 2,000,000 đ",
                        IconClass = "ti ti-credit-card",
                        AvatarBgClass = "bg-light-primary text-primary"
                    },
                    new DashboardActivityItem
                    {
                        Title = "Yêu cầu thanh toán hoa hồng",
                        Description = "HDV Trần Hoàng gửi yêu cầu hoa hồng 10% cho đoàn khách",
                        TimeAgo = "1 giờ trước",
                        AmountText = "720,000 đ",
                        IconClass = "ti ti-file-invoice",
                        AvatarBgClass = "bg-light-warning text-warning"
                    },
                    new DashboardActivityItem
                    {
                        Title = "Cập nhật menu món ăn",
                        Description = "Bếp trưởng vừa cập nhật trạng thái món: Chả giò Madame Lân",
                        TimeAgo = "2 giờ trước",
                        AmountText = "",
                        IconClass = "ti ti-tools-kitchen-2",
                        AvatarBgClass = "bg-light-info text-info"
                    }
                }
            };
        }
    }
}

