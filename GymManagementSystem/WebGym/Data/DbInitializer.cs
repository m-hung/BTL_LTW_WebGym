using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

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

            try
            {
                // 1. Kiểm tra và bổ sung mẫu giáo án do AI tạo (AI Smart Plan) nếu chưa có
                if (!context.WorkoutPlans.Any(p => p.Title.Contains("AI") || p.Goal.Contains("AI")))
                {
                    var firstMember = context.Members.FirstOrDefault();
                    var firstTrainer = context.Trainers.FirstOrDefault();
                    if (firstMember != null && firstTrainer != null)
                    {
                        context.WorkoutPlans.Add(new WorkoutPlan
                        {
                            Title = "[AI Smart] Lộ Trình Tối Ưu Hóa Tỷ Lệ Mỡ & Tim Mạch Thông Minh",
                            MemberId = firstMember.MemberId,
                            TrainerId = firstTrainer.TrainerId,
                            Goal = "AI Khuyến nghị: Giảm 4% mỡ nội tạng, tăng VO2Max qua phân tích chỉ số InBody và tần suất tim.",
                            StartDate = DateTime.UtcNow.Date,
                            EndDate = DateTime.UtcNow.Date.AddDays(60),
                            RoutineDetails = @"- Phân tích AI: Dựa trên chỉ số BMR 1.680 kcal và khối lượng cơ hiện tại.
- Thứ 2: AI Zone 2 Cardio (45 phút nhịp tim 125-135 bpm) + Lưng xô Lat Pulldown 3x12.
- Thứ 3: Full Body Functional Circuit: Kettlebell Swing 4x15, Goblet Squat 4x12, Push up 3x15.
- Thứ 4: Nghỉ phục hồi tích cực (Đi bộ dốc 4km/h, 30 phút).
- Thứ 5: AI HIIT Sprint trên máy chạy Technogym (30s chạy nhanh 15km/h, 60s đi bộ x 8 hiệp).
- Thứ 6: Upper Body Hypertrophy: Dumbbell Bench Press 4x10, Seated Cable Row 4x12, Bicep Curl 3x12.
- Thứ 7: Yoga & Giãn cơ phục hồi chuyển hóa cơ năng.
- Chủ nhật: AI Data Sync & Nghỉ ngơi.",
                            NutritionAdvice = @"- Mục tiêu năng lượng tính bởi AI Engine: 1.850 kcal/ngày (Deficit khoa học 15%).
- Đạm (Protein): 140g/ngày (30% tổng năng lượng), nạp đều sau buổi tập.
- Carb phức hợp: 190g/ngày (yến mạch, khoai lang vàng, gạo lứt).
- Chất béo tốt: 45g/ngày (Omega-3 từ dầu cá và quả bơ).
- Nước uống: Tối thiểu 3.2L/ngày, bổ sung điện giải trong phiên tập HIIT.",
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            IsDeleted = false
                        });
                        context.SaveChanges();
                    }
                }

                // 2. Đảm bảo có ít nhất 1 sản phẩm sắp hết hàng (< 5 cái) để hiển thị cảnh báo tồn kho
                var lowStockItem = context.Products.FirstOrDefault(p => p.StockQuantity > 0 && p.StockQuantity < 5);
                if (lowStockItem == null)
                {
                    var anyProduct = context.Products.OrderByDescending(p => p.ProductId).FirstOrDefault();
                    if (anyProduct != null && anyProduct.StockQuantity > 4)
                    {
                        anyProduct.StockQuantity = 3; // Tạo 1 sản phẩm còn 3 cái để thấy cảnh báo màu đỏ
                        context.SaveChanges();
                    }
                }
            }
            catch
            {
                // Bỏ qua lỗi runtime nếu DB đang bận
            }
        }
    }
}
