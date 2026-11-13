using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class EquipmentSeeder
    {
        public static void SeedEquipments(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Equipment>().HasData(GetData());
        }

        public static List<Equipment> GetData()
        {
            return new List<Equipment>
            {
                new Equipment
                {
                    EquipmentId = 1,
                    EquipmentName = "Máy chạy bộ Technogym Skillrun Unity 7000",
                    Area = "Khu cardio",
                    RoomId = 2,
                    Brand = "Technogym",
                    PurchaseDate = new DateTime(2025, 3, 15),
                    LastMaintenanceDate = new DateTime(2026, 10, 10),
                    Status = "Hoạt động tốt",
                    SerialNumber = "TG-SR7000-01",
                    ImageUrl = "https://images.unsplash.com/photo-1576678927484-cc907957088c?w=800&auto=format&fit=crop",
                    Notes = "Màn hình cảm ứng 19 inch, tích hợp bài tập leo dốc và chạy sức cản."
                },
                new Equipment
                {
                    EquipmentId = 2,
                    EquipmentName = "Máy chạy bộ Life Fitness Elevation Series Discover SE3",
                    Area = "Khu cardio",
                    RoomId = 2,
                    Brand = "Life Fitness",
                    PurchaseDate = new DateTime(2024, 8, 20),
                    LastMaintenanceDate = new DateTime(2026, 9, 25),
                    Status = "Cần bảo trì",
                    SerialNumber = "LF-SE3-02",
                    ImageUrl = "https://images.unsplash.com/photo-1576678927484-cc907957088c?w=800&auto=format&fit=crop",
                    Notes = "Băng chuyền phát tiếng kêu nhỏ khi nâng độ dốc trên 8%."
                },
                new Equipment
                {
                    EquipmentId = 3,
                    EquipmentName = "Xe đạp tập địa hình Matrix Fitness CXP Target Training",
                    Area = "Khu cardio",
                    RoomId = 2,
                    Brand = "Matrix Fitness",
                    PurchaseDate = new DateTime(2025, 5, 10),
                    LastMaintenanceDate = new DateTime(2026, 10, 05),
                    Status = "Hoạt động tốt",
                    SerialNumber = "MX-CXP-03",
                    ImageUrl = "https://images.unsplash.com/photo-1594737625785-a6cbdabd333c?w=800&auto=format&fit=crop",
                    Notes = "Xe đạp bánh đà kháng lực từ tính êm ái, đo công suất watt."
                },
                new Equipment
                {
                    EquipmentId = 4,
                    EquipmentName = "Máy chèo thuyền nước Concept2 Model D Rower",
                    Area = "Khu cardio",
                    RoomId = 2,
                    Brand = "Concept2",
                    PurchaseDate = new DateTime(2024, 11, 12),
                    LastMaintenanceDate = new DateTime(2026, 8, 15),
                    Status = "Đang hỏng",
                    SerialNumber = "C2-ROW-04",
                    ImageUrl = "https://images.unsplash.com/photo-1517838277536-f5f99be501cd?w=800&auto=format&fit=crop",
                    Notes = "Dây kéo xích bị kẹt puly trục cuốn, đang chờ linh kiện thay thế."
                },
                new Equipment
                {
                    EquipmentId = 5,
                    EquipmentName = "Khung gánh tạ trợ lực 3D Smith Machine",
                    Area = "Khu tạ tự do",
                    RoomId = 1,
                    Brand = "Impulse Fitness",
                    PurchaseDate = new DateTime(2024, 6, 18),
                    LastMaintenanceDate = new DateTime(2026, 10, 15),
                    Status = "Hoạt động tốt",
                    SerialNumber = "IMP-SM3D-05",
                    ImageUrl = "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=800&auto=format&fit=crop",
                    Notes = "Trục dẫn hướng chuyển động mượt mà, chốt khóa an toàn đa điểm."
                },
                new Equipment
                {
                    EquipmentId = 6,
                    EquipmentName = "Giàn tập Power Rack & Tạ đòn Olympic Rogue Monster Lite",
                    Area = "Khu tạ tự do",
                    RoomId = 1,
                    Brand = "Rogue Fitness",
                    PurchaseDate = new DateTime(2025, 1, 10),
                    LastMaintenanceDate = new DateTime(2026, 9, 30),
                    Status = "Hoạt động tốt",
                    SerialNumber = "ROG-PR-06",
                    ImageUrl = "https://images.unsplash.com/photo-1584735935682-2f2b69dff9d2?w=800&auto=format&fit=crop",
                    Notes = "Thép chịu lực tải trọng 500kg, kèm thanh xà đơn và chốt an toàn J-Cups."
                },
                new Equipment
                {
                    EquipmentId = 7,
                    EquipmentName = "Bộ giàn tạ tay Dumbbell Rogue Urethane 2.5kg - 50kg",
                    Area = "Khu tạ tự do",
                    RoomId = 1,
                    Brand = "Rogue Fitness",
                    PurchaseDate = new DateTime(2025, 1, 15),
                    LastMaintenanceDate = new DateTime(2026, 9, 20),
                    Status = "Hoạt động tốt",
                    SerialNumber = "ROG-DB-07",
                    ImageUrl = "https://images.unsplash.com/photo-1638803040283-7a5ffd48dad5?w=800&auto=format&fit=crop",
                    Notes = "Bọc Urethane chống ồn, tay cầm mạ chrome vân kim cương chống trượt."
                },
                new Equipment
                {
                    EquipmentId = 8,
                    EquipmentName = "Máy ép ngực & Kéo vai sau Pectoral Fly / Rear Deltoid",
                    Area = "Khu máy khối",
                    RoomId = 1,
                    Brand = "Technogym",
                    PurchaseDate = new DateTime(2024, 9, 05),
                    LastMaintenanceDate = new DateTime(2026, 10, 20),
                    Status = "Hoạt động tốt",
                    SerialNumber = "TG-SEL-08",
                    ImageUrl = "https://images.unsplash.com/photo-1581009146145-b5ef050c2e1e?w=800&auto=format&fit=crop",
                    Notes = "Hệ thống tạ tháp Selection Pro, chuyển đổi linh hoạt giữa ép ngực và kéo vai."
                },
                new Equipment
                {
                    EquipmentId = 9,
                    EquipmentName = "Máy đạp đùi nghiêng 45 độ Linear Leg Press",
                    Area = "Khu máy khối",
                    RoomId = 1,
                    Brand = "Life Fitness",
                    PurchaseDate = new DateTime(2024, 7, 22),
                    LastMaintenanceDate = new DateTime(2026, 8, 30),
                    Status = "Cần bảo trì",
                    SerialNumber = "LF-LP45-09",
                    ImageUrl = "https://images.unsplash.com/photo-1540497077202-7c8a3999166f?w=800&auto=format&fit=crop",
                    Notes = "Đệm lưng bị trầy rách góc dưới, ray trượt cần tra thêm mỡ bôi trơn công nghiệp."
                },
                new Equipment
                {
                    EquipmentId = 10,
                    EquipmentName = "Giàn cáp kéo xô đôi đa năng Dual Adjustable Pulley",
                    Area = "Khu chức năng",
                    RoomId = 1,
                    Brand = "Matrix Fitness",
                    PurchaseDate = new DateTime(2025, 2, 01),
                    LastMaintenanceDate = new DateTime(2026, 10, 28),
                    Status = "Hoạt động tốt",
                    SerialNumber = "MX-DAP-10",
                    ImageUrl = "https://images.unsplash.com/photo-1571902943202-507ec2618e8f?w=800&auto=format&fit=crop",
                    Notes = "Hai cột tạ độc lập, ròng rọc xoay 360 độ điều chỉnh độ cao nhiều mức."
                }
            };
        }
    }
}
