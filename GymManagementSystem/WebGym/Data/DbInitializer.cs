using Microsoft.EntityFrameworkCore;

namespace WebGym.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Tự động áp dụng Migration và Seed Data an toàn
            try
            {
                context.Database.Migrate();
            }
            catch
            {
                // Nếu migrate lỗi (ví dụ chưa bật SQL Server), fallback sang EnsureCreated
                context.Database.EnsureCreated();
            }

            // Đảm bảo cột AvatarUrl luôn tồn tại trong bảng Members
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM sys.columns 
                        WHERE object_id = OBJECT_ID('Members') AND name = 'AvatarUrl'
                    )
                    BEGIN
                        ALTER TABLE Members ADD AvatarUrl NVARCHAR(500) NULL;
                    END
                ");
            }
            catch
            {
                // Bỏ qua nếu đang chạy in-memory hoặc lỗi kết nối
            }

                        // Xóa các dữ liệu chỉ số cơ thể ảo để tài khoản mới tạo / chưa đo InBody hiển thị trống (--) theo yêu cầu
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    UPDATE Subscriptions SET PackageId = 1, OriginalPrice = 500000, PaidAmount = 500000, StartDate = '2026-11-01', EndDate = '2026-12-01' WHERE MemberId = 1;
                    DELETE FROM Bookings WHERE MemberId = 1;
                    DELETE FROM Progress;
                    UPDATE Members 
                    SET Height = NULL, 
                        Weight = NULL, 
                        BMR = NULL, 
                        BodyFatPercentage = NULL, 
                        MuscleMass = NULL, 
                        Chest = NULL, 
                        Waist = NULL, 
                        Hips = NULL;
                ");
            }
            catch
            {
                // Bỏ qua nếu đang chạy in-memory hoặc lỗi kết nối
            }
        }
    }
}
