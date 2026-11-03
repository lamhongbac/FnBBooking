---
name: state-preservation-guide
description: Hướng dẫn và quy chuẩn bảo toàn trạng thái tìm kiếm (Searching), bộ lọc (Filtering), sắp xếp (Sorting) và phân trang (Paging) cho các màn hình danh sách khi điều hướng hoặc thực hiện hành động Thêm/Sửa (Add/Edit) trong kiến trúc Blazor / FnBBooking.
---

# HƯỚNG DẪN BẢO TOÀN TRẠNG THÁI DANH SÁCH (STATE PRESERVATION GUIDE)

## 1. Mục tiêu (Objective)
Đảm bảo trải nghiệm người dùng liền mạch (Seamless UX):
- Khi người dùng đang ở một trang bất kỳ (ví dụ: **Trang 5**, bộ lọc trạng thái: **"Đang hoạt động"**, từ khóa tìm kiếm: **"VIP"**, sắp xếp theo cột **"Tên" Descending**).
- Thực hiện hành động **Thêm mới (New)** hoặc **Chỉnh sửa (Edit)** (thông qua Modal pop-up hoặc chuyển hướng sang View/URL Add-Edit riêng biệt).
- Khi đóng Form (Lưu thành công hoặc Bấm Hủy/Quay lại): **Hệ thống bắt buộc phải trở về chính xác Trang 5 với đúng toàn bộ từ khóa, bộ lọc và trạng thái sắp xếp trước đó.**

---

## 2. Kiến trúc giải pháp (Architecture Solution)

Hệ thống sử dụng mô hình **Scoped Dependency Injection Registry Service**:
```
[ User Interaction ] (Search / Filter / Sort / Page)
         │
         ▼
[ List Component / Razor Page ] ──(Sync)──► [ PageStateRegistryService (Scoped) ]
         │                                              │
         ▼                                              ▼
[ Add/Edit Action ]                               [ In-Memory Session Cache ]
(Modal or Navigation)                                  (Key: PageKey)
         │                                              │
         ▼                                              ▼
[ Return to List View ] ◄────────(Restore State)────────┘
```

### Thành phần cốt lõi:
1. **`MasterDataPageState.cs`** ([`FnBBooking.GUI.Services`](file:///d:/GIT%20PROJECT/FnBBooking/FnBBookingSln/FnBBooking.GUI/FnBBooking.GUI/Services/MasterDataPageState.cs)):
   - Lưu trữ các thông số:
     - `IsInitialized`: Đánh dấu state đã được khởi tạo lần đầu hay chưa.
     - `SearchKeyword`: Từ khóa tìm kiếm hiện tại.
     - `SelectedStatus`: Giá trị lọc trạng thái (`All`, `Active`, `Inactive`).
     - `CurrentPage`: Trang hiện tại đang xem.
     - `PageSize`: Số bản ghi trên mỗi trang (10, 20, 50, 100...).
     - `SortedProperty`: Tên thuộc tính / cột đang sort.
     - `SortedOrder`: Hướng sắp xếp (`Ascending` hoặc `Descending`).

2. **`PageStateRegistryService.cs`** ([`FnBBooking.GUI.Services`](file:///d:/GIT%20PROJECT/FnBBooking/FnBBookingSln/FnBBooking.GUI/FnBBooking.GUI/Services/PageStateRegistryService.cs)):
   - Đăng ký DI trong `Program.cs` dạng `Scoped`:
     ```csharp
     builder.Services.AddScoped<IPageStateRegistryService, PageStateRegistryService>();
     ```
   - Cung cấp các phương thức:
     - `GetOrCreateState(pageKey)`: Lấy hoặc tạo mới state cho màn hình tương ứng.
     - `SetState(pageKey, state)`: Lưu/cập nhật state cho màn hình.
     - `ResetState(pageKey)`: Xóa trắng state khi người dùng muốn reset về mặc định.
     - `GetCustomState<T>(pageKey)` & `SetCustomState<T>(pageKey, state)`: Hỗ trợ các bộ lọc phức tạp, đa trường (như trang Quản lý Booking).

---

## 3. Quy chuẩn triển khai trên một màn hình danh sách (Implementation Pattern)

### Bước 1: Khai báo Service & Định danh `PageKey`
```razor
@page "/my-manager"
@inject IPageStateRegistryService PageStateRegistry

@code {
    private const string PageKey = "MyManager"; // Duy nhất cho mỗi trang danh sách
    ...
```

### Bước 2: Khôi phục State trong `OnInitialized`
```csharp
protected override void OnInitialized()
{
    allItems = LoadData();

    // 1. Kiểm tra và khôi phục state đã lưu
    var savedState = PageStateRegistry.GetOrCreateState(PageKey);
    if (savedState.IsInitialized)
    {
        searchTerm = savedState.SearchKeyword;
        selectedStatus = savedState.SelectedStatus;
        pageSize = savedState.PageSize;
    }

    // 2. Thiết lập Sort Model và khôi phục cột/hướng sort
    var sortModel = new SortModel();
    sortModel.AddColumn("Number", isDefaultColumn: true);
    sortModel.AddColumn("Name");

    if (savedState.IsInitialized && !string.IsNullOrEmpty(savedState.SortedProperty))
    {
        sortModel.SortedProperty = savedState.SortedProperty;
        sortModel.SortedOrder = savedState.SortedOrder;
    }

    // 3. Thiết lập Filter Dictionary
    var filterDict = new Dictionary<string, object>
    {
        { FilterKey.SearchKeyword, searchTerm },
        { FilterKey.Status, selectedStatus }
    };

    // 4. Khởi tạo Controller với trang mục tiêu
    int targetPage = savedState.IsInitialized ? savedState.CurrentPage : 1;
    pageController = new PageMyController(allItems, sortModel, filterDict, targetPage, pageSize);
}
```

### Bước 3: Đồng bộ State ngược lại Service khi có thay đổi
Tạo một phương thức helper riêng:
```csharp
private void SaveCurrentPageState()
{
    if (pageController == null) return;
    var state = PageStateRegistry.GetOrCreateState(PageKey);
    state.SearchKeyword = searchTerm;
    state.SelectedStatus = selectedStatus;
    state.CurrentPage = pageController.PagingInfo.CurrentPage;
    state.PageSize = pageController.PagingInfo.PageSize;
    state.SortedProperty = pageController.PageSortModel.SortedProperty;
    state.SortedOrder = pageController.PageSortModel.SortedOrder;
    PageStateRegistry.SetState(PageKey, state);
}
```

Gọi `SaveCurrentPageState()` tại tất cả các sự kiện thay đổi:
- **`DoSearch()`**: Sau khi `pageController.SetupPage(1);`
- **`SortBy(prop)`**: Sau khi `pageController.SetupPage(...);`
- **`HandlePageChange(page)`**: Sau khi `pageController.GoToPage(page);`
- **`HandlePageSizeChange(newSize)`**: Sau khi `pageController.ChangePageSize(newSize);`
- **`HandleSaved(savedItem)`**: Sau khi `pageController.RefreshPage();`

---

## 4. Kiểm chứng chất lượng (Verification Checklist)
- [x] Người dùng ở trang 3, thực hiện chỉnh sửa 1 dòng -> Lưu -> Màn hình vẫn ở trang 3 với dòng vừa sửa.
- [x] Người dùng lọc trạng thái "Ngừng hoạt động", chuyển sang xem trang khác, rồi quay lại trang danh sách ban đầu -> Bộ lọc vẫn giữ nguyên "Ngừng hoạt động".
- [x] Xóa thành công 1 bản ghi -> Hệ thống tự tính toán lại tổng số trang: nếu trang hiện tại vượt quá tổng số trang mới, tự động lùi về trang cuối hợp lệ.
- [x] Biên dịch: Không có cảnh báo lỗi về DI Container hay null reference.

