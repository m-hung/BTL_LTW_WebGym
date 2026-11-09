using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class ProductCategorySeeder
    {
        public static void SeedProductCategories(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasData(GetData());
        }

        public static List<ProductCategory> GetData()
        {
            return new List<ProductCategory>
            {
                new ProductCategory
                {
                    CategoryId = 1,
                    CategoryName = "Thực phẩm bổ sung & Whey Protein",
                    Description = "Sữa tăng cơ Whey Isolate, Hydrolyzed, Mass Gainer hỗ trợ phục hồi và phát triển cơ bắp tối ưu.",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryId = 2,
                    CategoryName = "Tăng sức mạnh & Năng lượng (Pre-Workout & BCAA)",
                    Description = "Thức uống bùng nổ sức mạnh Pre-Workout, Creatine Monohydrate và điện giải BCAA chống mỏi cơ.",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryId = 3,
                    CategoryName = "Dụng cụ & Phụ kiện tập Gym",
                    Description = "Găng tay, đai lưng nâng tạ bọc da, dây kéo lưng Lifting Straps, bình lắc Shaker và băng quấn cổ tay.",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryId = 4,
                    CategoryName = "Trang phục thể thao Gym & Athleisure",
                    Description = "Áo thun co giãn thoáng khí, quần legging, áo Tanktop khoét nách và trang phục tập luyện cao cấp.",
                    IsActive = true
                },
                new ProductCategory
                {
                    CategoryId = 5,
                    CategoryName = "Nước giải khát thể thao & Bữa phụ dinh dưỡng",
                    Description = "Thanh Protein Bar ít đường, nước bù khoáng điện giải Isotonic và hạt sấy khô bổ sung năng lượng nhanh.",
                    IsActive = true
                }
            };
        }
    }
}
