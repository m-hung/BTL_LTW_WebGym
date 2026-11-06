using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class ProgressSeeder
    {
        public static void SeedProgress(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            var data = GetData();
            if (data.Any())
            {
                modelBuilder.Entity<Progress>().HasData(data);
            }
        }

        public static List<Progress> GetData()
        {
            // Tài khoản mới tạo / chưa đo InBody sẽ không có bản ghi tiến trình (để trống theo yêu cầu)
            return new List<Progress>();
        }
    }
}
