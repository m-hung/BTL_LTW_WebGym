using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class ClassBookingSeeder
    {
        public static void SeedClassBookings(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassBooking>().HasData(GetData());
        }

        public static List<ClassBooking> GetData()
        {
            return new List<ClassBooking>
            {
                new ClassBooking
                {
                    ClassBookingId = 1,
                    ClassId = 1, // Hatha Yoga
                    MemberId = 2, // Đặng Thanh Hương
                    BookingDate = new DateTime(2026, 11, 2, 6, 0, 0),
                    Status = "Attended",
                    Notes = "Hội viên tham gia đầy đủ buổi tập Yoga sáng."
                },
                new ClassBooking
                {
                    ClassBookingId = 2,
                    ClassId = 1, // Hatha Yoga
                    MemberId = 4, // Vũ Phương Anh
                    BookingDate = new DateTime(2026, 11, 2, 6, 0, 0),
                    Status = "Attended",
                    Notes = "Mượn thảm tập cao su non của phòng."
                },
                new ClassBooking
                {
                    ClassBookingId = 3,
                    ClassId = 2, // Spinning RPM
                    MemberId = 1, // Nguyễn Quang Hải
                    BookingDate = new DateTime(2026, 11, 3, 17, 30, 0),
                    Status = "Attended",
                    Notes = "Đạt mục tiêu đốt 650 kcal."
                },
                new ClassBooking
                {
                    ClassBookingId = 4,
                    ClassId = 2, // Spinning RPM
                    MemberId = 3, // Phạm Đức Thắng
                    BookingDate = new DateTime(2026, 11, 3, 17, 30, 0),
                    Status = "Cancelled",
                    Notes = "Báo bận lịch công tác đột xuất lúc 15:00."
                },
                new ClassBooking
                {
                    ClassBookingId = 5,
                    ClassId = 3, // BodyPump
                    MemberId = 1, // Nguyễn Quang Hải
                    BookingDate = new DateTime(2026, 11, 13, 18, 30, 0),
                    Status = "Confirmed",
                    Notes = "Xác nhận tham gia lớp BodyPump tối thứ 6."
                },
                new ClassBooking
                {
                    ClassBookingId = 6,
                    ClassId = 3, // BodyPump
                    MemberId = 5, // Lê Hoàng Nam
                    BookingDate = new DateTime(2026, 11, 13, 18, 30, 0),
                    Status = "Confirmed",
                    Notes = "Đã đặt chỗ thành công."
                },
                new ClassBooking
                {
                    ClassBookingId = 7,
                    ClassId = 4, // Kickfit
                    MemberId = 3, // Phạm Đức Thắng
                    BookingDate = new DateTime(2026, 11, 14, 19, 30, 0),
                    Status = "Confirmed",
                    Notes = "Đã xác nhận giữ chỗ lớp đấm bốc."
                },
                new ClassBooking
                {
                    ClassBookingId = 8,
                    ClassId = 5, // Pilates
                    MemberId = 2, // Đặng Thanh Hương
                    BookingDate = new DateTime(2026, 11, 14, 8, 30, 0),
                    Status = "Confirmed",
                    Notes = "Học viên yêu cầu điều chỉnh tư thế thắt lưng."
                },
                new ClassBooking
                {
                    ClassBookingId = 9,
                    ClassId = 5, // Pilates
                    MemberId = 4, // Vũ Phương Anh
                    BookingDate = new DateTime(2026, 11, 14, 8, 30, 0),
                    Status = "Confirmed",
                    Notes = "Đã xếp máy Reformer số 3."
                },

                // Class 1 (Hatha Yoga - Max 20, Open): thêm 12 Confirmed => 12 Confirmed (+2 Attended)
                new ClassBooking { ClassBookingId = 11, ClassId = 1, MemberId = 25, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Đăng ký học Yoga buổi sáng." },
                new ClassBooking { ClassBookingId = 12, ClassId = 1, MemberId = 26, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Hội viên mang thảm cá nhân." },
                new ClassBooking { ClassBookingId = 13, ClassId = 1, MemberId = 27, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Giãn cơ phục hồi sau chạy bộ." },
                new ClassBooking { ClassBookingId = 14, ClassId = 1, MemberId = 28, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Đăng ký qua cổng hội viên." },
                new ClassBooking { ClassBookingId = 15, ClassId = 1, MemberId = 29, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Tập trung cải thiện nhịp thở." },
                new ClassBooking { ClassBookingId = 16, ClassId = 1, MemberId = 30, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Đăng ký lớp HLV Elena." },
                new ClassBooking { ClassBookingId = 17, ClassId = 1, MemberId = 31, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Đăng ký giữ chỗ trước." },
                new ClassBooking { ClassBookingId = 18, ClassId = 1, MemberId = 32, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Giữ chỗ lớp sáng." },
                new ClassBooking { ClassBookingId = 19, ClassId = 1, MemberId = 33, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Giảm căng cứng vai gáy." },
                new ClassBooking { ClassBookingId = 20, ClassId = 1, MemberId = 34, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Tham gia cùng nhóm bạn." },
                new ClassBooking { ClassBookingId = 21, ClassId = 1, MemberId = 35, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Đăng ký tại quầy lễ tân." },
                new ClassBooking { ClassBookingId = 22, ClassId = 1, MemberId = 36, BookingDate = new DateTime(2026, 11, 13, 6, 30, 0), Status = "Confirmed", Notes = "Đăng ký qua website." },

                // Class 2 (Spinning RPM - Max 25, Open): thêm 15 Confirmed => 15 Confirmed (+1 Attended, 1 Cancelled)
                new ClassBooking { ClassBookingId = 23, ClassId = 2, MemberId = 25, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Đạp xe đốt mỡ tối thứ 5." },
                new ClassBooking { ClassBookingId = 24, ClassId = 2, MemberId = 26, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Đăng ký giữ xe hàng đầu." },
                new ClassBooking { ClassBookingId = 25, ClassId = 2, MemberId = 27, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Lớp Cardio cùng HLV Marcus." },
                new ClassBooking { ClassBookingId = 26, ClassId = 2, MemberId = 28, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Chuẩn bị sẵn bình nước điện giải." },
                new ClassBooking { ClassBookingId = 27, ClassId = 2, MemberId = 29, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Mục tiêu đốt 700 kcal." },
                new ClassBooking { ClassBookingId = 28, ClassId = 2, MemberId = 30, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Đăng ký online." },
                new ClassBooking { ClassBookingId = 29, ClassId = 2, MemberId = 31, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Đăng ký qua cổng hội viên." },
                new ClassBooking { ClassBookingId = 30, ClassId = 2, MemberId = 32, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Tăng sức bền tim mạch." },
                new ClassBooking { ClassBookingId = 31, ClassId = 2, MemberId = 33, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Giữ chỗ lớp tối." },
                new ClassBooking { ClassBookingId = 32, ClassId = 2, MemberId = 34, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Tập luyện theo nhịp nhạc RPM." },
                new ClassBooking { ClassBookingId = 33, ClassId = 2, MemberId = 35, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Đặt chỗ tại quầy." },
                new ClassBooking { ClassBookingId = 34, ClassId = 2, MemberId = 36, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Đăng ký trực tuyến." },
                new ClassBooking { ClassBookingId = 35, ClassId = 2, MemberId = 37, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Tham gia đều 3 buổi/tuần." },
                new ClassBooking { ClassBookingId = 36, ClassId = 2, MemberId = 38, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Giữ chỗ thành công." },
                new ClassBooking { ClassBookingId = 37, ClassId = 2, MemberId = 39, BookingDate = new DateTime(2026, 11, 12, 18, 0, 0), Status = "Confirmed", Notes = "Tập cardio giảm mỡ." },

                // Class 3 (BodyPump - Max 25, Full): 2 Confirmed (#5, #6) + 23 Confirmed => 25/25 Full
                new ClassBooking { ClassBookingId = 38, ClassId = 3, MemberId = 2, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đăng ký lớp BodyPump HLV Liam." },
                new ClassBooking { ClassBookingId = 39, ClassId = 3, MemberId = 3, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Giữ vị trí khu vực trung tâm." },
                new ClassBooking { ClassBookingId = 40, ClassId = 3, MemberId = 4, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Sẵn sàng bộ tạ 15kg." },
                new ClassBooking { ClassBookingId = 41, ClassId = 3, MemberId = 21, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đăng ký qua cổng hội viên." },
                new ClassBooking { ClassBookingId = 42, ClassId = 3, MemberId = 22, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Tập sức bền thân trên & đùi." },
                new ClassBooking { ClassBookingId = 43, ClassId = 3, MemberId = 23, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đã xác nhận tham gia." },
                new ClassBooking { ClassBookingId = 44, ClassId = 3, MemberId = 24, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Tăng sức bền toàn thân." },
                new ClassBooking { ClassBookingId = 45, ClassId = 3, MemberId = 25, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Giữ chỗ lớp BodyPump tối." },
                new ClassBooking { ClassBookingId = 46, ClassId = 3, MemberId = 26, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đặt chỗ thành công." },
                new ClassBooking { ClassBookingId = 47, ClassId = 3, MemberId = 27, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đăng ký tại quầy lễ tân." },
                new ClassBooking { ClassBookingId = 48, ClassId = 3, MemberId = 28, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đăng ký online." },
                new ClassBooking { ClassBookingId = 49, ClassId = 3, MemberId = 29, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Tập theo giáo án sức bền." },
                new ClassBooking { ClassBookingId = 50, ClassId = 3, MemberId = 30, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Xác nhận tham gia." },
                new ClassBooking { ClassBookingId = 51, ClassId = 3, MemberId = 31, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đã đăng ký trước 3 ngày." },
                new ClassBooking { ClassBookingId = 52, ClassId = 3, MemberId = 32, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Tập tạ nhóm buổi tối." },
                new ClassBooking { ClassBookingId = 53, ClassId = 3, MemberId = 33, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Giữ chỗ lớp tối." },
                new ClassBooking { ClassBookingId = 54, ClassId = 3, MemberId = 34, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đăng ký cùng bạn tập." },
                new ClassBooking { ClassBookingId = 55, ClassId = 3, MemberId = 35, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đặt chỗ trực tuyến." },
                new ClassBooking { ClassBookingId = 56, ClassId = 3, MemberId = 36, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Xác nhận giữ chỗ." },
                new ClassBooking { ClassBookingId = 57, ClassId = 3, MemberId = 37, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đặt trước qua App/Web." },
                new ClassBooking { ClassBookingId = 58, ClassId = 3, MemberId = 38, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Đã xác nhận slot." },
                new ClassBooking { ClassBookingId = 59, ClassId = 3, MemberId = 39, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Sẵn sàng buổi BodyPump." },
                new ClassBooking { ClassBookingId = 60, ClassId = 3, MemberId = 40, BookingDate = new DateTime(2026, 11, 13, 18, 30, 0), Status = "Confirmed", Notes = "Slot cuối cùng của lớp BodyPump." },

                // Class 4 (Kickfit - Max 15, Open): 1 Confirmed (#7) + 7 Confirmed => 8/15 Open
                new ClassBooking { ClassBookingId = 61, ClassId = 4, MemberId = 1, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Tập phản xạ Boxing cùng HLV Devon." },
                new ClassBooking { ClassBookingId = 62, ClassId = 4, MemberId = 5, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Mượn găng Boxing 12oz của phòng." },
                new ClassBooking { ClassBookingId = 63, ClassId = 4, MemberId = 22, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Đăng ký buổi tối thứ 5/CN." },
                new ClassBooking { ClassBookingId = 64, ClassId = 4, MemberId = 24, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Tăng thể lực tim mạch & đấm bao cát." },
                new ClassBooking { ClassBookingId = 65, ClassId = 4, MemberId = 41, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Đăng ký qua lễ tân." },
                new ClassBooking { ClassBookingId = 66, ClassId = 4, MemberId = 42, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Học kỹ thuật tự vệ cơ bản." },
                new ClassBooking { ClassBookingId = 67, ClassId = 4, MemberId = 43, BookingDate = new DateTime(2026, 11, 14, 19, 30, 0), Status = "Confirmed", Notes = "Đặt chỗ thành công." },

                // Class 5 (Pilates - Max 12, Full): 2 Confirmed (#8, #9) + 10 Confirmed => 12/12 Full
                new ClassBooking { ClassBookingId = 68, ClassId = 5, MemberId = 21, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Xếp máy Reformer số 1." },
                new ClassBooking { ClassBookingId = 69, ClassId = 5, MemberId = 23, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Tập cùng HLV Chloe Bennett." },
                new ClassBooking { ClassBookingId = 70, ClassId = 5, MemberId = 26, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Cải thiện độ linh hoạt lưng dưới." },
                new ClassBooking { ClassBookingId = 71, ClassId = 5, MemberId = 28, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Đăng ký lớp cuối tuần." },
                new ClassBooking { ClassBookingId = 72, ClassId = 5, MemberId = 30, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Giữ chỗ máy Reformer số 5." },
                new ClassBooking { ClassBookingId = 73, ClassId = 5, MemberId = 32, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Tập săn chắc cơ bụng Core." },
                new ClassBooking { ClassBookingId = 74, ClassId = 5, MemberId = 34, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Đăng ký online thành công." },
                new ClassBooking { ClassBookingId = 75, ClassId = 5, MemberId = 36, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Lớp trị liệu tư thế sáng cuối tuần." },
                new ClassBooking { ClassBookingId = 76, ClassId = 5, MemberId = 38, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Đã xác nhận slot." },
                new ClassBooking { ClassBookingId = 77, ClassId = 5, MemberId = 40, BookingDate = new DateTime(2026, 11, 14, 8, 30, 0), Status = "Confirmed", Notes = "Slot cuối cùng của lớp Pilates." }
            };
        }
    }
}
