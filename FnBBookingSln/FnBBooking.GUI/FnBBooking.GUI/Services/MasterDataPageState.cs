using System;
using FnBBooking.GUI.Helpers;

namespace FnBBooking.GUI.Services
{
    /// <summary>
    /// Đối tượng lưu trữ trạng thái tìm kiếm, lọc, sắp xếp và phân trang của một trang danh sách Master Data.
    /// </summary>
    public class MasterDataPageState
    {
        public bool IsInitialized { get; set; } = false;
        public string SearchKeyword { get; set; } = string.Empty;
        public string SelectedStatus { get; set; } = "All";
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortedProperty { get; set; } = "Number";
        public SortOrder SortedOrder { get; set; } = SortOrder.Ascending;
    }
}
