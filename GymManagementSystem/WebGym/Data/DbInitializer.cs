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

            // Tự động kiểm tra và thêm cột AvatarUrl cho bảng Members nếu chưa có
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM sys.columns 
                        WHERE object_id = OBJECT_ID(N'[dbo].[Members]') 
                          AND name = N'AvatarUrl'
                    )
                    BEGIN
                        ALTER TABLE [dbo].[Members] ADD [AvatarUrl] NVARCHAR(500) NULL;
                    END
                ");
            }
            catch
            {
                // Bỏ qua nếu dùng provider khác hoặc đã tồn tại
            }
        }
    }
}
