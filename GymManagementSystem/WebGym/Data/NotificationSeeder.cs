using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class NotificationSeeder
    {
        public static void SeedNotifications(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>().HasData(GetData());
        }

        public static List<Notification> GetData()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = 1,
                    UserId = 1, // Admin
                    Title = "Thông báo sự cố thiết bị cần xử lý",
                    Message = "Máy chèo thuyền nước Concept2 (Khu cardio - Tầng 2) đang gặp sự cố kẹt xích kéo, đã tạo phiếu bảo trì.",
                    Type = "Maintenance",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 1, 9, 0, 0),
                    TargetUrl = "/Admin/Equipment"
                },
                new Notification
                {
                    NotificationId = 2,
                    UserId = 1, // Admin
                    Title = "Đơn hàng sản phẩm mới phát sinh",
                    Message = "Hội viên Lê Hoàng Nam vừa đặt mua Đơn hàng #5 trị giá 2,470,000đ cần kiểm tra xuất kho.",
                    Type = "System",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 11, 6, 18, 25, 0),
                    TargetUrl = "/Admin/Order"
                },
                new Notification
                {
                    NotificationId = 3,
                    UserId = 2, // HLV Trần Minh Nhật
                    Title = "Lịch đặt tập PT 1:1 mới",
                    Message = "Hội viên Nguyễn Quang Hải đã đặt lịch tập ngực & thể lực vào ngày mai lúc 18:00.",
                    Type = "Booking",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 11, 2, 14, 0, 0),
                    TargetUrl = "/Trainer/Bookings"
                },
                new Notification
                {
                    NotificationId = 4,
                    UserId = 2, // HLV Trần Minh Nhật
                    Title = "Lớp học BodyPump đã đủ sĩ số tối đa",
                    Message = "Lớp BodyPump Thứ 4 (18:30) của bạn đã đạt tối đa 25/25 học viên đăng ký giữ chỗ.",
                    Type = "Class",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 4, 12, 0, 0),
                    TargetUrl = "/Trainer/Classes"
                },
                new Notification
                {
                    NotificationId = 5,
                    UserId = 5, // Member Nguyễn Quang Hải
                    Title = "Chúc mừng! Bạn đã hoàn thành mốc InBody mới",
                    Message = "Chỉ số mỡ cơ thể của bạn đã giảm xuống 18.5%, giảm 7.0% so với ngày bắt đầu. Hãy tiếp tục phát huy!",
                    Type = "System",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 10, 30, 20, 0, 0),
                    TargetUrl = "/Member/Progress"
                },
                new Notification
                {
                    NotificationId = 6,
                    UserId = 5, // Member Nguyễn Quang Hải
                    Title = "Kế hoạch tập luyện tuần mới sẵn sàng",
                    Message = "HLV Trần Minh Nhật vừa cập nhật giáo án tập luyện 4 buổi/tuần và thực đơn tăng cơ nạc cho bạn.",
                    Type = "System",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 5, 8, 30, 0),
                    TargetUrl = "/Member/WorkoutPlan"
                },
                new Notification
                {
                    NotificationId = 7,
                    UserId = 6, // Member Đặng Thanh Hương
                    Title = "Nhắc nhở lịch học Yoga ngày mai",
                    Message = "Bạn có lịch tham gia lớp Hatha Yoga & Thiền Định lúc 06:30 sáng mai tại Studio 1.",
                    Type = "Class",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 7, 20, 0, 0),
                    TargetUrl = "/Member/ClassBooking"
                },
                new Notification
                {
                    NotificationId = 8,
                    UserId = 7, // Member Phạm Đức Thắng
                    Title = "Đăng ký thành công lớp Kickfit Combat",
                    Message = "Bạn đã đăng ký thành công lớp Kickfit lúc 19:30 Thứ 5. Vui lòng mang găng tay và khăn tập cá nhân.",
                    Type = "Class",
                    IsRead = true,
                    CreatedAt = new DateTime(2026, 11, 5, 15, 0, 0),
                    TargetUrl = "/Member/ClassBooking"
                },
                new Notification
                {
                    NotificationId = 9,
                    UserId = 8, // Member Vũ Phương Anh
                    Title = "Đơn hàng #4 đang được vận chuyển",
                    Message = "Đơn hàng phụ kiện Gym của bạn đã được xuất kho và đang trên đường giao tới địa chỉ đăng ký.",
                    Type = "System",
                    IsRead = false,
                    CreatedAt = new DateTime(2026, 11, 3, 11, 0, 0),
                    TargetUrl = "/Member/Orders"
                }
            };
        }
    }
}
