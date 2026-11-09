# HƯỚNG DẪN HỢP TÁC & PHÂN CHIA CÔNG VIỆC DỰ ÁN WEBGYM (NHÓM 5 THÀNH VIÊN)
**Môn học:** Lập Trình Web - ASP.NET Core MVC (.NET 10)  
**Tên Solution:** `GymManagementSystem` | **Project MVC:** `WebGym`

---

## 1. CƠ SỞ DỮ LIỆU ĐÃ CẬP NHẬT (DATABASE SCHEMA TOÀN DIỆN)

### A. Nhóm Quản Lý Người Dùng & Hội Viên
1. **Users:** `UserId`, `Username`, `PasswordHash`, `FullName`, `Email`, `PhoneNumber`, `Role` ('Admin', 'PT', 'Member'), `CreatedAt`.
2. **Members:** `MemberId`, `UserId` (FK -> Users), `Gender`, `DateOfBirth`, `Height`, `Weight` (ban đầu), `BMR`, `BodyFatPercentage`, `MuscleMass`, `Chest`, `Waist`, `Hips`.
3. **Trainers:** `TrainerId`, `UserId` (FK -> Users), `Specialization`, `Bio`, `AvatarUrl`.
4. **GymPackages:** `PackageId`, `PackageName`, `Description`, `Price`, `DurationDays`, `IncludesPT` (bool).
5. **Subscriptions:** `SubscriptionId`, `MemberId` (FK -> Members), `PackageId` (FK -> GymPackages), `OriginalPrice`, `PaidAmount`, `StartDate`, `EndDate`, `Status` ('Active', 'Expired', 'Cancelled').
6. **Bookings:** `BookingId`, `MemberId` (FK -> Members), `TrainerId` (FK -> Trainers), `BookingDate`, `TimeSlot`, `Status` ('Pending', 'Approved', 'Completed', 'Cancelled'), `Notes`.
7. **Progress:** `ProgressId`, `MemberId` (FK -> Members), `RecordedDate`, `Weight`, `BodyFatPercentage`, `MuscleMass`, `Chest`, `Waist`, `Hips`, `Notes`.
8. **Payments:** `PaymentId`, `MemberId` (FK -> Members), `PackageId` (FK -> GymPackages), `Amount`, `PaymentMethod` ('Momo', 'VNPay', 'BankTransfer'), `TransactionCode`, `PaymentDate`, `Status` ('Success', 'Failed').

### B. Nhóm Quản Lý Cơ Sở Vật Chất & Check-in (Mới)
9. **CheckIns:** `CheckInId`, `MemberId` (FK -> Members), `CheckInTime`, `CheckOutTime`, `Method` ('Mã QR Code'), `Notes`.
10. **GymRooms:** `RoomId`, `RoomName`, `Location`, `Capacity`, `Status` ('Hoạt động', 'Bảo trì', 'Tạm đóng'), `Description`, `CreatedAt`.
11. **Equipments:** `EquipmentId`, `EquipmentName`, `Area` ('Khu tạ tự do', 'Khu cardio', 'Khu máy khối', 'Khu chức năng'), `RoomId` (FK -> GymRooms), `Brand`, `PurchaseDate`, `LastMaintenanceDate`, `Status` ('Hoạt động tốt', 'Cần bảo trì', 'Đang hỏng'), `SerialNumber`, `ImageUrl`, `Notes`.
12. **MaintenanceLogs:** `LogId`, `EquipmentId` (FK -> Equipments), `MaintenanceDate`, `Description`, `Cost`, `PerformedBy`, `Status` ('Hoạt động', 'Đang bảo dưỡng', 'Chờ linh kiện'), `Notes`.

### C. Nhóm Cửa Hàng & Đơn Hàng Sản Phẩm (Mới)
13. **ProductCategories:** `CategoryId`, `CategoryName`, `Description`, `IsActive`.
14. **Products:** `ProductId`, `CategoryId` (FK -> ProductCategories), `ProductName`, `ImageUrl`, `Price`, `StockQuantity`, `Description`, `IsActive`, `CreatedAt`.
15. **Orders:** `OrderId`, `MemberId` (FK -> Members), `UserId` (FK -> Users), `OrderDate`, `TotalAmount`, `Status` ('Chờ xác nhận', 'Đã thanh toán', 'Đang giao', 'Hoàn thành', 'Đã hủy'), `PaymentMethod` ('Tiền mặt', 'VNPay', 'Momo', 'Chuyển khoản'), `ShippingAddress`, `PhoneNumber`, `Notes`.
16. **OrderDetails:** `OrderDetailId`, `OrderId` (FK -> Orders), `ProductId` (FK -> Products), `Quantity`, `UnitPrice`, `TotalPrice`.

### D. Nhóm Lớp Học Nhóm, Thông Báo & Kế Hoạch Tập Luyện (Mới)
17. **GymClasses:** `ClassId`, `ClassName`, `Description`, `TrainerId` (FK -> Trainers), `RoomId` (FK -> GymRooms), `Schedule`, `MaxCapacity`, `CurrentCapacity`, `Status` ('Open', 'Full', 'Completed', 'Cancelled'), `StartDate`, `EndDate`, `ImageUrl`.
18. **ClassBookings:** `ClassBookingId`, `ClassId` (FK -> GymClasses), `MemberId` (FK -> Members), `BookingDate`, `Status` ('Confirmed', 'Attended', 'Cancelled'), `Notes`.
19. **Notifications:** `NotificationId`, `UserId` (FK -> Users), `Title`, `Message`, `Type` ('System', 'Booking', 'Class', 'Payment', 'Maintenance', 'Promotion'), `IsRead`, `CreatedAt`, `TargetUrl`.
20. **WorkoutPlans:** `Id` / `PlanId`, `Title`, `MemberId` (FK -> Members), `TrainerId` (FK -> Trainers), `Goal`, `StartDate`, `EndDate`, `RoutineDetails` (Lịch tập chi tiết), `NutritionAdvice` (Thực đơn & calo), `IsActive`, `CreatedAt`, `UpdatedAt`, `IsDeleted`.

---

## 2. DỮ LIỆU MẪU ĐƯỢC CẤU HÌNH SẴN (SEED DATA CHO BÁO CÁO & DEMO)

- **4 Gói tập GymPackages:** Gói Đồng (500k), Bạc (1.3tr), Vàng (4.5tr), Kim Cương (8tr).
- **Tài khoản thuyết trình & đăng nhập:**
  - **Admin:** `admin@gym.com` / `Admin@123` (hoặc username: `admin`)
  - **PT 1 (Có lịch tập & lớp BodyPump):** `minhnhat.pt@gym.com` / `PT@123` (Trần Minh Nhật)
  - **PT 2 (Phụ trách HIIT, Spinning & Kickfit):** `hoanganh.pt@gym.com` / `PT@123` (Lê Hoàng Anh)
  - **PT 3 (Phụ trách Yoga & Pilates):** `thuytien.pt@gym.com` / `PT@123` (Nguyễn Thụy Tiên)
  - **Hội viên 1 (InBody tiến trình, Check-in, Kế hoạch tập 12 tuần):** `quanghai@gmail.com` / `Member@123` (Nguyễn Quang Hải)
  - **Hội viên 2, 3, 4, 5, 21, 22, 23, 24:** Mật khẩu chung `Member@123`.

- **5 Phòng tập GymRooms:** Phòng Gym chính Tầng 1, Studio Cardio & Spinning Tầng 2, Studio Yoga & Pilates Tầng 3, Combat Zone Kickfit Tầng 3, PT Private Studio Tầng 2.
- **10 Máy móc & Thiết bị Equipments:** Technogym Skillrun, Life Fitness Treadmill, Matrix CXP Bike, Concept2 Rower, Smith Machine, Rogue Power Rack, Bộ tạ tay Dumbbell, Máy ép ngực, Máy đạp đùi Leg Press, Giàn cáp đôi DAP. Đầy đủ trạng thái: Hoạt động tốt, Cần bảo trì, Đang hỏng.
- **7 Bản ghi bảo dưỡng MaintenanceLogs:** Nhật ký thay cáp, tra mỡ trục, cân thảm chạy, bọc da đệm, sửa xích máy chèo.
- **12 Lượt Check-in:** Tất cả sử dụng phương thức quét Mã QR Code từ ứng dụng/cổng quét.
- **5 Danh mục & 10 Sản phẩm Products:** Whey Gold Standard, Rule 1 Isolate, Creatine MuscleTech, C4 Pre-workout, BCAA Xtend, Đai lưng Lever Belt da thật, Găng tay Harbinger, Shaker BlenderBottle, Áo GymTech Pro, Protein Bar Quest.
- **5 Đơn hàng Orders & 9 Chi tiết OrderDetails:** Đơn hàng thanh toán VNPay, Momo, Tiền mặt với đầy đủ trạng thái.
- **5 Lớp học GymClasses & 9 Lượt đặt lớp ClassBookings:** Yoga, Spinning RPM, BodyPump, Kickfit Combat, Pilates Reformer.
- **9 Thông báo Notifications:** Báo cáo hỏng máy cho Admin, lịch dạy lớp cho PT, nhắc tập & xác nhận đơn hàng cho Member.
- **3 Kế hoạch tập WorkoutPlans:** Giáo án 12 tuần tăng cơ giảm mỡ, siết eo săn chắc nữ, sức mạnh Powerlifting.

---

## 3. LỆNH CẬP NHẬT DATABASE CHO THÀNH VIÊN KHÁC

Khi bất kỳ thành viên nào kéo code mới từ Git về:

### Sử dụng .NET CLI (Terminal / PowerShell):
```bash
cd WebGym
dotnet ef database update
```

### Hoặc Package Manager Console trong Visual Studio:
```powershell
Update-Database -Project WebGym
```
*(Nếu cần tạo lại database từ đầu: `dotnet ef database drop -f` rồi `dotnet ef database update`)*
