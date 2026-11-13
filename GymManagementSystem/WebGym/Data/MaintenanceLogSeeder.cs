using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class MaintenanceLogSeeder
    {
        public static void SeedMaintenanceLogs(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceLog>().HasData(GetData());
        }

        public static List<MaintenanceLog> GetData()
        {
            return new List<MaintenanceLog>
            {
                new MaintenanceLog
                {
                    LogId = 1,
                    EquipmentId = 1,
                    MaintenanceDate = new DateTime(2026, 10, 10),
                    Description = "Bảo dưỡng định kỳ 6 tháng: kiểm tra motor 4.0 HP, vệ sinh bo mạch và tra dầu silicon chuyên dụng băng tải.",
                    Cost = 850000m,
                    PerformedBy = "Technogym Vietnam Services",
                    Status = "Hoàn thành",
                    Notes = "Thiết bị vận hành êm ái, đạt chuẩn thông số kỹ thuật xuất xưởng."
                },
                new MaintenanceLog
                {
                    LogId = 2,
                    EquipmentId = 2,
                    MaintenanceDate = new DateTime(2026, 9, 25),
                    Description = "Kiểm tra độ chùng thảm chạy, cân chỉnh lực căng 2 trục con lăn và vệ sinh cảm biến nhịp tim cầm tay.",
                    Cost = 600000m,
                    PerformedBy = "Kỹ thuật viên phòng máy (Nguyễn Văn Bình)",
                    Status = "Hoàn thành",
                    Notes = "Khuyến nghị thay băng tải trong đợt bảo dưỡng tới do mài mòn lớp cao su."
                },
                new MaintenanceLog
                {
                    LogId = 3,
                    EquipmentId = 4,
                    MaintenanceDate = new DateTime(2026, 8, 15),
                    Description = "Sự cố kẹt xích kéo máy chèo thuyền Concept2: tháo dỡ cụm bánh đà quạt gió, kiểm tra lò xo hồi xích.",
                    Cost = 1450000m,
                    PerformedBy = "Trung tâm sửa chữa thiết bị thể thao Việt Á",
                    Status = "Đang bảo dưỡng",
                    Notes = "Đã đặt mua lò xo cuộn và xích mạ niken chính hãng từ Mỹ, dự kiến hoàn tất trong 5 ngày."
                },
                new MaintenanceLog
                {
                    LogId = 4,
                    EquipmentId = 5,
                    MaintenanceDate = new DateTime(2026, 10, 15),
                    Description = "Bôi trơn 2 thanh ray trục Smith bằng mỡ trắng lithium, kiểm tra các chốt hãm lò xo an toàn chống rơi.",
                    Cost = 350000m,
                    PerformedBy = "Bảo trì nội bộ Gym",
                    Status = "Hoàn thành",
                    Notes = "Không có hư hỏng cơ khí. Cân bằng trọng lực đòn tạ chuẩn."
                },
                new MaintenanceLog
                {
                    LogId = 5,
                    EquipmentId = 8,
                    MaintenanceDate = new DateTime(2026, 10, 20),
                    Description = "Thay thế dây cáp bọc nhựa đường kính 5mm của tháp tạ ép ngực do bị xước lớp vỏ bảo vệ bên ngoài.",
                    Cost = 750000m,
                    PerformedBy = "Kỹ thuật viên Lê Quốc Dũng",
                    Status = "Hoàn thành",
                    Notes = "Dây cáp mới chịu tải 1500kg, đã test tải trọng tối đa 100kg an toàn."
                },
                new MaintenanceLog
                {
                    LogId = 6,
                    EquipmentId = 9,
                    MaintenanceDate = new DateTime(2026, 8, 30),
                    Description = "Bọc lại lớp da simili cao cấp cho đệm tựa lưng máy đạp đùi và thay thế 2 bạc đạn trượt linear bearing.",
                    Cost = 1200000m,
                    PerformedBy = "Xưởng cơ khí & da thể thao Thành Phát",
                    Status = "Hoàn thành",
                    Notes = "Mặt đệm êm, đường may đôi chắc chắn, khả năng chịu mồ hôi tốt."
                },
                new MaintenanceLog
                {
                    LogId = 7,
                    EquipmentId = 10,
                    MaintenanceDate = new DateTime(2026, 10, 28),
                    Description = "Cân chỉnh rãnh xoay ròng rọc 360 độ, kiểm tra chốt chọn tạ từ tính và siết ốc cố định chân đế sàn bê tông.",
                    Cost = 400000m,
                    PerformedBy = "Bảo trì nội bộ Gym",
                    Status = "Hoàn thành",
                    Notes = "Tất cả các nấc nạp cáp trượt trơn tru."
                },

                // === BỔ SUNG BẢO TRÌ THIẾT BỊ TRẢI RỘNG CÁC THÁNG NĂM 2026 ===
                new MaintenanceLog { LogId = 9, EquipmentId = 1, MaintenanceDate = new DateTime(2026, 1, 20), Description = "Kiểm tra tổng quát máy chạy bộ Technogym đầu năm, hiệu chuẩn độ nghiêng 15% và vệ sinh quạt tản nhiệt.", Cost = 650000m, PerformedBy = "Technogym Vietnam Services", Status = "Hoàn thành", Notes = "Hoạt động ổn định." },
                new MaintenanceLog { LogId = 10, EquipmentId = 3, MaintenanceDate = new DateTime(2026, 2, 18), Description = "Vệ sinh cụm cảm biến từ tính kháng lực xe đạp Matrix CXP và siết chặt bàn đạp.", Cost = 450000m, PerformedBy = "Bảo trì nội bộ Gym", Status = "Hoàn thành", Notes = "Đo công suất Watt chính xác." },
                new MaintenanceLog { LogId = 11, EquipmentId = 6, MaintenanceDate = new DateTime(2026, 3, 14), Description = "Kiểm tra chốt an toàn J-hooks và thanh chắn trên khung Power Rack, sơn dặm chống gỉ phần đế.", Cost = 500000m, PerformedBy = "Xưởng cơ khí Thành Phát", Status = "Hoàn thành", Notes = "Đảm bảo tải trọng 400kg." },
                new MaintenanceLog { LogId = 12, EquipmentId = 7, MaintenanceDate = new DateTime(2026, 4, 22), Description = "Siết bulong giá đỡ giàn tạ đơn, thay mới đệm cao su chống va đập trên 6 cặp tạ từ 20-35kg.", Cost = 900000m, PerformedBy = "Bảo trì nội bộ Gym", Status = "Hoàn thành", Notes = "Giảm tiếng ồn khi hạ tạ." },
                new MaintenanceLog { LogId = 13, EquipmentId = 2, MaintenanceDate = new DateTime(2026, 5, 19), Description = "Thay thảm chạy và kiểm tra bo mạch công suất máy chạy bộ Life Fitness sau 6 tháng vận hành cao điểm.", Cost = 2200000m, PerformedBy = "Kỹ thuật viên phòng máy (Nguyễn Văn Bình)", Status = "Hoàn thành", Notes = "Thảm chạy mới êm ái, độ bám tốt." },
                new MaintenanceLog { LogId = 14, EquipmentId = 8, MaintenanceDate = new DateTime(2026, 6, 16), Description = "Tra dầu puly ròng rọc máy ép ngực và vệ sinh thanh dẫn trượt chồng tạ 90kg.", Cost = 400000m, PerformedBy = "Bảo trì nội bộ Gym", Status = "Hoàn thành", Notes = "Chuyển động mượt." },
                new MaintenanceLog { LogId = 15, EquipmentId = 9, MaintenanceDate = new DateTime(2026, 7, 25), Description = "Kiểm tra cơ cấu khóa an toàn 2 bên của máy đạp đùi Leg Press 45 độ và thay 2 chốt lò xo.", Cost = 800000m, PerformedBy = "Xưởng cơ khí & da thể thao Thành Phát", Status = "Hoàn thành", Notes = "Đạt chuẩn an toàn kỹ thuật." },
                new MaintenanceLog { LogId = 16, EquipmentId = 3, MaintenanceDate = new DateTime(2026, 9, 10), Description = "Thay mới yên đệm gel thể thao và kiểm tra bộ truyền động dây curoa xe đạp Matrix CXP.", Cost = 700000m, PerformedBy = "Bảo trì nội bộ Gym", Status = "Hoàn thành", Notes = "Phục vụ lớp Spinning RPM." },
                new MaintenanceLog { LogId = 17, EquipmentId = 4, MaintenanceDate = new DateTime(2026, 11, 4), Description = "Hoàn tất thay thế cụm xích mạ niken và hiệu chỉnh màn hình PM5 máy chèo thuyền Concept2.", Cost = 1100000m, PerformedBy = "Trung tâm sửa chữa thiết bị thể thao Việt Á", Status = "Hoàn thành", Notes = "Đã nghiệm thu bàn giao đưa vào sử dụng." },
                new MaintenanceLog { LogId = 18, EquipmentId = 1, MaintenanceDate = new DateTime(2026, 11, 11), Description = "Bảo dưỡng định kỳ trước đợt cao điểm cuối năm: vệ sinh motor, bôi trơn băng tải máy chạy Technogym.", Cost = 550000m, PerformedBy = "Technogym Vietnam Services", Status = "Hoàn thành", Notes = "Máy vận hành ổn định 100%." }
            };
        }
    }
}
