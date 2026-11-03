# CHUẨN HÓA MÀN HÌNH DANH SÁCH (MASTER DATA LIST STANDARD)

Tài liệu này quy định kiến trúc và cách thức triển khai chuẩn cho tất cả các màn hình danh mục (Master Data) trong hệ thống **FnBBooking.GUI**. Mọi màn hình danh sách bắt buộc phải tuân theo cấu trúc này để đảm bảo tính đồng bộ, dễ mở rộng và dễ bảo trì.

---

## 1. CÁC THÀNH PHẦN KIẾN TRÚC CỐT LÕI

Mỗi màn hình Master Data bao gồm 5 thành phần chuẩn:

```
[ Data Source / ViewModel ]
             │
             ▼
[ PageController (kế thừa BasePageController<T>) ]
   ├── Filter (Tìm kiếm, Trạng thái)
   ├── DoSort (Sắp xếp theo cột)
   └── Paging (Phân trang dữ liệu)
             │
             ▼
[ Razor Page (Màn hình danh sách) ]
   ├── Thanh tìm kiếm & Bộ lọc (Search / Filter Toolbar)
   ├── Bảng dữ liệu có Header click Sort & Sort Icon
   ├── Component Phân Trang (<PaginationComponent />)
   └── Reusable Modal Add/Edit (<TModal />)
```

---

## 2. QUY ĐỊNH CẤU TRÚC DỮ LIỆU CHUẨN

Mọi Model/ViewModel Master Data đều kế thừa từ `BaseMasterDataViewModel`:

* `Id`: `int` (Khóa chính)
* `Number`: `string` (Mã số / Code, varchar(20), Unique, Bắt buộc)
* `Name`: `string` (Tên, nvarchar(250), Bắt buộc)
* `Description`: `string?` (Mô tả ghi chú, nvarchar(500))
* `IsActive`: `bool` (Trạng thái hoạt động, mặc định `true`)
* `IsDeleted`: `bool` (Đánh dấu xóa mềm, mặc định `false`)
* **Audit Metadata**:
  * `CreatedOn`, `ModifiedOn`: `DateTime` (Mặc định ngày giờ hiện tại)
  * `CreatedBy`, `ModifiedBy`: `string` (Gán tự động từ `AppConfig:CurrentLoginId` trong `appsettings.json`)

---

## 3. BỘ CÔNG CỤ ĐIỀU KHIỂN TRANG (PAGE CONTROLLER)

### 3.1. Các lớp Helper (`FnBBooking.GUI.Helpers`)
* `CodesByAnizTools.cs`:
  * `SortModel`: Quản lý danh sách cột cho phép sort (`SortableColumns`), cột đang chọn (`SortedProperty`) và thứ tự (`SortOrder`).
  * `PagerModel`: Tự động tính toán số trang (`TotalPages`), phân bổ số trang (`StartPage`, `EndPage`).
  * `PaginatedList<T>`: Tập con dữ liệu được phân trang từ danh sách đã lọc.
* `BasePageController<T>`:
  * Quản lý `AllItems`, `FilterItems`, `FilterObject`, `CurrentPageData`, `PageSortModel`, `PagingInfo`.
  * Cung cấp các phương thức: `GoToPage(int page)`, `ChangePageSize(int size)`, `SetupPage()`, `RefreshPage()`.
  * Bắt buộc Controller con override `Filter()` và `DoSort()`.

### 3.2. Mẫu tạo PageController cho từng danh mục
Ví dụ cho `Area`:
```csharp
public class PageAreaController : BasePageController<AreaViewModel>
{
    public PageAreaController(List<AreaViewModel> allItems, SortModel sortModel,
        Dictionary<string, object> filterObject, int pageIndex = 1, int pageSize = 10)
        : base(allItems, sortModel, filterObject, pageIndex, pageSize) { }

    public override void Filter()
    {
        // 1. Lọc xóa mềm (IsDeleted = false)
        // 2. Lọc theo từ khóa tìm kiếm (Number, Name)
        // 3. Lọc theo trạng thái (IsActive)
        DoSort();
    }

    public override void DoSort()
    {
        // Sắp xếp theo PageSortModel.SortedProperty và PageSortModel.SortedOrder
    }
}
```

---

## 4. QUY CHUẨN GIAO DIỆN MÀN HÌNH DANH SÁCH (RAZOR PAGE)

Mỗi file màn hình danh sách (ví dụ: `AreaManager.razor`) bao gồm các khối chuẩn:

### 4.1. Tiêu đề trang & Nút "Thêm Mới"
* Tiêu đề có icon Tabler và mô tả ngắn gọn.
* Nút "Thêm mới" kích hoạt component Modal riêng biệt: `@onclick="() => modalRef?.ShowCreate()"`.

### 4.2. Thanh tìm kiếm & Bộ lọc (Toolbar với Searching & Filtering)
* **Nguyên tắc**: Toàn bộ các trường nhập liệu trên Toolbar (ô text tìm kiếm, dropdown trạng thái, phân loại, v.v.) đóng vai trò là **Filter Parameters**.
* **Nút Tìm kiếm ("Tìm kiếm" / Search button)**:
  * Khi người dùng bấm **Nút "Tìm kiếm"** (hoặc nhấn phím **Enter** trên ô input), hệ thống sẽ **gom tất cả các giá trị filter hiện có làm parameters** để kích hoạt truy vấn lại dữ liệu (`DoSearch()`), làm mới nguồn dữ liệu (`AllItems`) và setup lại phân trang về trang 1.
  * Có nút phụ "Đặt lại" (Reset / Clear) để xóa từ khóa và đưa các bộ lọc về mặc định.
* **Giao diện mẫu**:
  ```razor
  <div class="row g-2 align-items-center">
      <div class="col-md-5">
          <div class="input-group">
              <span class="input-group-text bg-white"><i class="ti ti-search"></i></span>
              <input type="text" class="form-control" placeholder="Tìm theo mã hoặc tên..."
                     @bind="searchTerm" @onkeyup="HandleKeyUp" />
              <button class="btn btn-primary" type="button" @onclick="DoSearch">
                  <i class="ti ti-search me-1"></i> Tìm kiếm
              </button>
          </div>
      </div>
      <div class="col-md-4">
          <div class="d-flex align-items-center gap-2">
              <label class="text-muted small text-nowrap mb-0">Trạng thái:</label>
              <select class="form-select" @bind="selectedStatus">
                  <option value="All">Tất cả trạng thái</option>
                  <option value="Active">Đang hoạt động</option>
                  <option value="Inactive">Ngừng hoạt động</option>
              </select>
          </div>
      </div>
      <div class="col-md-3 text-md-end text-muted small">
          Tổng tìm thấy: <strong>@pageController.PagingInfo.TotalItems</strong> bản ghi
      </div>
  </div>
  ```

### 4.3. Bảng dữ liệu (Table)
* **Header cột có thể sắp xếp**:
  ```razor
  <th @onclick='() => SortBy("Number")' style="cursor:pointer">
      MÃ KHU VỰC <i class="@GetSortIcon("Number")"></i>
  </th>
  ```
* **Icon sắp xếp chuẩn**:
  * Chưa sort: `ti ti-arrows-sort text-muted`
  * Tăng dần: `ti ti-sort-ascending text-primary`
  * Giảm dần: `ti ti-sort-descending text-primary`
* **Dữ liệu**: Bind lặp từ `@pageController.CurrentPageData`.
* **Cột Thao tác**: Nút Sửa gọi `modalRef?.ShowEdit(item)` và nút Xóa mở modal xác nhận xóa mềm.

### 4.4. Phân trang (`PaginationComponent.razor`)
* Đặt dưới chân bảng dữ liệu:
  ```razor
  <PaginationComponent currentPage="@pageController.PagingInfo.CurrentPage"
                       totalPages="@pageController.PagingInfo.TotalPages"
                       totalItems="@pageController.CurrentPageData.TotalRecords"
                       pageSize="@pageSize"
                       OnPageChanged="HandlePageChange"
                       OnPageSizeChanged="HandlePageSizeChange" />
  ```

### 4.5. Reusable Modal Add/Edit
* Màn hình Add/Edit **bắt buộc tách thành component độc lập** trong `Components\Modals` (ví dụ `AreaModal.razor`).
* Nhúng vào trang danh sách:
  ```razor
  <AreaModal @ref="areaModal" OnSaved="HandleAreaSaved" />
  ```
* Cho phép tái sử dụng ở các màn hình khác (ví dụ màn hình TableManager nhúng `AreaModal` để tạo nhanh Area qua nút `+`).

---

## 5. CHECKLIST NGHIỆM THU CHO MỖI MÀN HÌNH MASTER DATA

- [ ] Đã có ViewModel kế thừa `BaseMasterDataViewModel`.
- [ ] Đã có `Page[Entity]Controller` kế thừa `BasePageController<T>`.
- [ ] Đã có component Modal riêng biệt trong `Components\Modals`.
- [ ] Màn hình danh sách hỗ trợ Tìm kiếm tức thì (`Filter`).
- [ ] Header bảng hỗ trợ Sắp xếp tăng/giảm kèm icon trực quan (`Sort`).
- [ ] Đã tích hợp `PaginationComponent` hỗ trợ đổi trang và đổi số dòng/trang (`Paging`).
- [ ] Khi Thêm/Sửa/Xóa thành công, tự động cập nhật danh sách và hiển thị alert thông báo.

