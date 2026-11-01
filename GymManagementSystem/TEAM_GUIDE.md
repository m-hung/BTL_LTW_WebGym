# HƯỚNG DẪN HỢP TÁC & PHÂN CHIA CÔNG VIỆC DỰ ÁN WEBGYM (NHÓM 5 THÀNH VIÊN)
**Môn học:** Lập Trình Web - ASP.NET Core MVC (.NET 10)  
**Tên Solution:** `GymManagementSystem` | **Project MVC:** `WebGym`

---

## 1. CƠ SỞ DỮ LIỆU ĐÃ CẬP NHẬT (DATABASE SCHEMA)

1. **Users:** `UserId`, `Username`, `PasswordHash`, `FullName`, `Email`, `PhoneNumber`, `Role` ('Admin', 'PT', 'Member'), `CreatedAt`.
2. **Members:** `MemberId`, `UserId` (FK -> Users), `Gender`, `DateOfBirth`, `Height`, `Weight` (ban đầu), `BMR`, `BodyFatPercentage`, `MuscleMass`, `Chest`, `Waist`, `Hips`.
3. **Trainers:** `TrainerId`, `UserId` (FK -> Users), `Specialization`, `Bio`, `AvatarUrl`.
4. **GymPackages:** `PackageId`, `PackageName`, `Description`, `Price`, `DurationDays`, `IncludesPT` (bool).
5. **Subscriptions:** `SubscriptionId`, `MemberId` (FK -> Members), `PackageId` (FK -> GymPackages), `OriginalPrice`, `PaidAmount`, `StartDate`, `EndDate`, `Status` ('Active', 'Expired', 'Cancelled').
6. **Bookings:** `BookingId`, `MemberId` (FK -> Members), `TrainerId` (FK -> Trainers), `BookingDate`, `TimeSlot`, `Status` ('Pending', 'Approved', 'Completed', 'Cancelled'), `Notes`.
7. **Progress:** `ProgressId`, `MemberId` (FK -> Members), `RecordedDate`, `Weight`, `BodyFatPercentage`, `MuscleMass`, `Chest`, `Waist`, `Hips`, `Notes`.
8. **Payments:** `PaymentId`, `MemberId` (FK -> Members), `PackageId` (FK -> GymPackages), `Amount`, `PaymentMethod` ('Momo', 'VNPay', 'BankTransfer'), `TransactionCode`, `PaymentDate`, `Status` ('Success', 'Failed').

---

## 2. DỮ LIỆU MẪU ĐƯỢC CẤU HÌNH SẴN (SEED DATA CHO BÁO CÁO & DEMO)

- **4 Gói tập GymPackages:**
  1. *Gói Đồng (Basic 1 Tháng):* 500,000 VND - 30 ngày - IncludesPT = false.
  2. *Gói Bạc (Standard 3 Tháng):* 1,300,000 VND - 90 ngày - IncludesPT = false.
  3. *Gói Vàng (VIP 6 Tháng kèm PT):* 4,500,000 VND - 180 ngày - IncludesPT = true.
  4. *Gói Kim Cương (VVIP 1 Năm kèm PT):* 8,000,000 VND - 365 ngày - IncludesPT = true.

- **Tài khoản thuyết trình & đăng nhập:**
  - **Admin:** `admin@gym.com` / `Admin@123` (hoặc username: `admin`)
  - **PT 1 (Có 10 lịch tập):** `minhnhat.pt@gym.com` / `PT@123` (Trần Minh Nhật)
  - **PT 2:** `hoanganh.pt@gym.com` / `PT@123` (Lê Hoàng Anh)
  - **PT 3:** `thuytien.pt@gym.com` / `PT@123` (Nguyễn Thụy Tiên)
  - **Hội viên 1 (Có 6 mốc InBody tiến trình):** `quanghai@gmail.com` / `Member@123` (Nguyễn Quang Hải)
  - **Hội viên 2, 3, 4, 5:** `thanhhuong@gmail.com`, `ducthang@gmail.com`, `phuonganh@gmail.com`, `hoangnam@gmail.com` (mật khẩu chung: `Member@123`)

- **Doanh thu rải rác 6 tháng:** Đã có 11 giao dịch mẫu (Momo, VNPay, BankTransfer với các mức 500k, 1.3tr, 4.5tr, 8tr) để vẽ biểu đồ báo cáo trên Admin Dashboard.
- **Tiến trình InBody (Hội viên 1):** 6 mốc từ 78.5kg -> 70.2kg, % mỡ 25.5% -> 18.5%, vòng eo 88cm -> 77.5cm.
- **Lịch tập Bookings (HLV 1):** 10 ca tập rải rác trong tuần đủ trạng thái `Pending`, `Approved`, `Completed`, `Cancelled`.

---

## 3. LỆNH CHẠY MIGRATION CẬP NHẬT DATABASE

Khi Leader MH hoặc bất kỳ thành viên nào kéo code về máy:

### Cách 1: Sử dụng .NET CLI (Terminal / PowerShell):
```bash
cd WebGym
dotnet ef migrations add UpdateDatabaseSchemaAndSeedData
dotnet ef database update
```

### Cách 2: Sử dụng Package Manager Console (trong Visual Studio):
```powershell
Add-Migration UpdateDatabaseSchemaAndSeedData -Project WebGym
Update-Database -Project WebGym
```
