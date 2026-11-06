using Microsoft.EntityFrameworkCore;

namespace WebGym.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                // Thực hiện migrate database nếu có migration mới
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

            // Khôi phục gói tập và đảm bảo không ghi đè gói tập nâng cấp của hội viên
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    -- Nếu Member 1 có gói tập giá trị 4.500.000đ (Gói Vàng), đảm bảo PackageId = 3 đúng gói
                    UPDATE Subscriptions 
                    SET PackageId = 3, 
                        Status = 'Active'
                    WHERE MemberId = 1 AND (PaidAmount >= 4500000 OR OriginalPrice >= 4500000 OR EndDate > '2027-01-01');

                    UPDATE Payments 
                    SET PackageId = 3, 
                        Status = 'Success'
                    WHERE MemberId = 1 AND Amount >= 4500000;
                ");
            }
            catch
            {
                // Bỏ qua nếu đang chạy in-memory hoặc lỗi kết nối
            }
        }
    }
}
