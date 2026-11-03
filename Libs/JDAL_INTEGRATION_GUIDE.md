# Hướng dẫn Tích hợp và Sử dụng JDALLib (Qua File DLL)

Tài liệu này hướng dẫn chi tiết cách biên dịch, sao chép file `JDALLib.dll` và tích hợp vào một dự án .NET bất kỳ (ASP.NET Core Web API / MVC, Worker Service, Console App hoặc Desktop App WinForms/WPF) để sử dụng JSON làm local database mà không cần cài đặt hệ quản trị CSDL (SQL Server, MySQL,...).

---

## 1. Chuẩn bị file `JDALLib.dll`

### 1.1. Biên dịch ra bản Release
Tại thư mục gốc của dự án `JDALLib`, mở Terminal / PowerShell và chạy lệnh:
```bash
dotnet build -c Release
```

Sau khi build thành công, file DLL sẽ nằm tại:
```text
JDALLib/bin/Release/netstandard2.1/JDALLib.dll
```

> **Target Framework**: `netstandard2.1` tương thích với mọi dự án chạy **.NET Core 3.0+, .NET 5, .NET 6, .NET 7, .NET 8, .NET 9+**.

---

## 2. Tích hợp DLL vào Dự án Mới

### Bước 1: Sao chép file DLL vào dự án
1. Tạo một thư mục chứa thư viện ngoài trong dự án mới, ví dụ: `libs/` hoặc `Dependencies/`.
2. Sao chép file `JDALLib.dll` vào thư mục `libs/`.

```text
MyProject/
├── Controllers/
├── Models/
├── libs/
│   └── JDALLib.dll     <-- Đặt tại đây
├── appsettings.json
├── Program.cs
└── MyProject.csproj
```

### Bước 2: Thêm Reference trong file `.csproj`
Mở file `.csproj` của dự án mới và thêm khai báo `<Reference>` trỏ tới file DLL:

```xml
<ItemGroup>
  <Reference Include="JDALLib">
    <HintPath>libs\JDALLib.dll</HintPath>
  </Reference>
</ItemGroup>
```

*(Hoặc trên Visual Studio: Chuột phải vào **Dependencies** -> chọn **Add Project Reference...** -> chọn tab **Browse** -> chọn file `libs/JDALLib.dll`)*.

### Bước 3: Cài đặt các gói phụ thuộc (Dependencies)
Do `JDALLib` sử dụng thư viện Serialization và Data Annotations, hãy đảm bảo dự án đích đã cài các package cần thiết:

```bash
dotnet add package Newtonsoft.Json --version 13.0.3
dotnet add package System.ComponentModel.Annotations --version 5.0.0
```
> **Lưu ý**: Đối với dự án .NET 6 / 7 / 8 / 9, `System.Text.Json` đã có sẵn trong runtime nên không cần cài thêm.

---

## 3. Hướng dẫn Lập trình & Sử dụng

### Bước 1: Định nghĩa Model Entity
Mọi Model lưu trữ qua `JDALLib` cần kế thừa từ lớp `BaseDataObject` (đã có sẵn khóa chính `public int Id { get; set; }`).

```csharp
using JDALLib;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class Product : BaseDataObject
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
```

---

### Bước 2: Tạo DB Context (`JDBContext`)
Tạo một lớp kế thừa từ `JDBContext` để quản lý các tập dữ liệu (`JDBSet<T>`).

```csharp
using JDALLib;
using MyProject.Models;

namespace MyProject.Data
{
    public class AppDbContext : JDBContext
    {
        // Khai báo bảng Products
        public JDBSet<Product> Products { get; }

        public AppDbContext(IConfiguration configuration, IWebHostEnvironment env)
        {
            // Lấy tên thư mục chứa data từ cấu hình hoặc mặc định là "Data"
            string dataFolder = configuration["JDALConfig:DataPath"] ?? "Data";

            // QUAN TRỌNG: Dùng ContentRootPath để lưu ngoài wwwroot (tránh bị lộ file)
            string rootPath = env.ContentRootPath;

            // Khởi tạo và cấu hình đường dẫn thư mục lưu trữ
            ContextConfig(rootPath, dataFolder);

            // Khởi tạo DbSet tương ứng (tự động tạo Product.json)
            Products = new JDBSet<Product>(_fullDataPath);
        }
    }
}
```

> **Đối với Console App / Desktop App (không có IWebHostEnvironment)**:
> Bạn chỉ cần truyền thư mục ứng dụng:
> ```csharp
> string rootPath = AppDomain.CurrentDomain.BaseDirectory;
> ContextConfig(rootPath, "Data");
> Products = new JDBSet<Product>(_fullDataPath);
> ```

---

### Bước 3: Cấu hình `appsettings.json` và `Program.cs`

#### Trong `appsettings.json`:
```json
{
  "JDALConfig": {
    "DataPath": "Data"
  }
}
```

#### Trong `Program.cs` (Đăng ký Dependency Injection):
`JDALLib` đã được tối ưu hóa đa luồng với `ReaderWriterLockSlim`, nên bạn có thể an tâm đăng ký dạng **Singleton**:

```csharp
using MyProject.Data;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký AppDbContext dạng Singleton
builder.Services.AddSingleton<AppDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();
// ...
app.Run();
```

---

### Bước 4: Thực hiện các thao tác CRUD (Create, Read, Update, Delete)

Có thể inject trực tiếp `AppDbContext` vào Controller hoặc Service:

```csharp
using Microsoft.AspNetCore.Mvc;
using MyProject.Data;
using MyProject.Models;

namespace MyProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // 1. READ ALL & LINQ QUERY
        public IActionResult Index(string keyword)
        {
            // Lấy toàn bộ danh sách (trả về bản sao an toàn)
            var products = _context.Products.ToList();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                // Dùng LINQ truy vấn trực tiếp trên JDBSet
                products = _context.Products
                    .Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(products);
        }

        // 2. READ BY ID (O(1) Fast Lookup)
        public IActionResult Details(int id)
        {
            Product? product = _context.Products.Find(id);
            if (product == null) return NotFound();

            return View(product);
        }

        // 3. CREATE
        [HttpPost]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            // Hệ thống tự động sinh ID tiếp theo (Auto-increment)
            bool isSuccess = _context.Products.Create(model);
            
            // Hoặc dùng phiên bản Async:
            // bool isSuccess = await _context.Products.CreateAsync(model);

            if (isSuccess)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Không thể lưu sản phẩm.");
            return View(model);
        }

        // 4. UPDATE
        [HttpPost]
        public IActionResult Edit(Product model)
        {
            if (!ModelState.IsValid) return View(model);

            // Cập nhật bản ghi có Id trùng khớp
            bool isSuccess = _context.Products.Update(model);
            // Hoặc await _context.Products.UpdateAsync(model);

            if (isSuccess)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Không tìm thấy hoặc không thể cập nhật.");
            return View(model);
        }

        // 5. DELETE
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Product? product = _context.Products.Find(id);
            if (product == null) return NotFound();

            bool isSuccess = _context.Products.Delete(product);
            // Hoặc await _context.Products.DeleteAsync(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
```

---

## 4. Các điểm lưu ý quan trọng

| Vấn đề | Khuyến nghị |
| :--- | :--- |
| **Bảo mật file JSON** | **Tuyệt đối không lưu dữ liệu trong thư mục `wwwroot`**. Hãy lưu tại `Data/` hoặc `App_Data/` ở cấp Root ứng dụng để tránh bị người dùng truy cập trực tiếp qua trình duyệt. |
| **Độ tin cậy ghi file** | `JDALLib` đã triển khai cơ chế **Atomic File Write** (ghi ra `.tmp` rồi hoán đổi file chính). Dữ liệu sẽ không bao giờ bị corrupt nếu xảy ra sự cố sập nguồn hoặc tắt app đột ngột. |
| **An toàn đa luồng** | `JDBSet<T>` đã tích hợp `ReaderWriterLockSlim` cho phép nhiều luồng đọc đồng thời và một luồng ghi tuần tự. An toàn 100% khi chạy trong môi trường ASP.NET Core Singleton. |
| **Khóa chính Id** | `Id` được tự động sinh tăng dần bắt đầu từ `1`. Khi tạo mới thực thể, bạn **không cần gán giá trị Id**. |
| **Tự động Rollback** | Nếu quá trình ghi đĩa bị lỗi, `JDBSet<T>` sẽ tự động phục hồi lại trạng thái danh sách trong bộ nhớ (RAM). |
