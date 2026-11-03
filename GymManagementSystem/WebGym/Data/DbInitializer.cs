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
        }
    }
}
