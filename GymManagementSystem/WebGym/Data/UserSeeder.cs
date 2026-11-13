using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class UserSeeder
    {
        // Passwords hash (SHA-256 base64)
        // Admin@123   -> 6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=
        // PT@123      -> T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=
        // Member@123  -> q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=
        public const string AdminHash = "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=";
        public const string PtHash = "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=";
        public const string MemberHash = "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=";

        public static void SeedUsers(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(GetData());
        }

        public static List<User> GetData()
        {
            return new List<User>
            {
                // Admin
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = AdminHash,
                    FullName = "System Administrator",
                    Email = "admin@gym.com",
                    PhoneNumber = "0901234567",
                    Role = "Admin",
                    CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                // 3 PTs
                new User
                {
                    UserId = 2,
                    Username = "pt_minhnhat",
                    PasswordHash = PtHash,
                    FullName = "Trần Minh Nhật",
                    Email = "minhnhat.pt@gym.com",
                    PhoneNumber = "0912345678",
                    Role = "PT",
                    CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 3,
                    Username = "pt_hoanganh",
                    PasswordHash = PtHash,
                    FullName = "Lê Hoàng Anh",
                    Email = "hoanganh.pt@gym.com",
                    PhoneNumber = "0912345679",
                    Role = "PT",
                    CreatedAt = new DateTime(2026, 5, 2, 0, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 4,
                    Username = "pt_thuytien",
                    PasswordHash = PtHash,
                    FullName = "Nguyễn Thụy Tiên",
                    Email = "thuytien.pt@gym.com",
                    PhoneNumber = "0912345680",
                    Role = "PT",
                    CreatedAt = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc)
                },
                // 5 Western PTs for 5 Group Classes
                new User { UserId = 10, Username = "pt_elena", PasswordHash = PtHash, FullName = "Elena Rostova", Email = "elena.pt@gym.com", PhoneNumber = "0912345681", Role = "PT", CreatedAt = new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 11, Username = "pt_marcus", PasswordHash = PtHash, FullName = "Marcus Vance", Email = "marcus.pt@gym.com", PhoneNumber = "0912345682", Role = "PT", CreatedAt = new DateTime(2026, 5, 4, 0, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 12, Username = "pt_liam", PasswordHash = PtHash, FullName = "Liam O'Connor", Email = "liam.pt@gym.com", PhoneNumber = "0912345683", Role = "PT", CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 13, Username = "pt_devon", PasswordHash = PtHash, FullName = "Devon Brooks", Email = "devon.pt@gym.com", PhoneNumber = "0912345684", Role = "PT", CreatedAt = new DateTime(2026, 5, 5, 0, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 14, Username = "pt_chloe", PasswordHash = PtHash, FullName = "Chloe Bennett", Email = "chloe.pt@gym.com", PhoneNumber = "0912345685", Role = "PT", CreatedAt = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc) },
                // 5 Members
                new User
                {
                    UserId = 5,
                    Username = "member_quanghai",
                    PasswordHash = MemberHash,
                    FullName = "Nguyễn Quang Hải",
                    Email = "quanghai@gmail.com",
                    PhoneNumber = "0987654321",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 1, 10, 8, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 6,
                    Username = "member_thanhhuong",
                    PasswordHash = MemberHash,
                    FullName = "Đặng Thanh Hương",
                    Email = "thanhhuong@gmail.com",
                    PhoneNumber = "0987654322",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 6, 15, 9, 30, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 7,
                    Username = "member_ducthang",
                    PasswordHash = MemberHash,
                    FullName = "Vũ Đức Thắng",
                    Email = "ducthang@gmail.com",
                    PhoneNumber = "0987654323",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 5, 10, 14, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 8,
                    Username = "member_phuonganh",
                    PasswordHash = MemberHash,
                    FullName = "Bùi Phương Anh",
                    Email = "phuonganh@gmail.com",
                    PhoneNumber = "0987654324",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 9, 5, 10, 15, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 9,
                    Username = "member_hoangnam",
                    PasswordHash = MemberHash,
                    FullName = "Phạm Hoàng Nam",
                    Email = "hoangnam@gmail.com",
                    PhoneNumber = "0987654325",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 7, 20, 16, 45, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 21,
                    Username = "member_baongoc",
                    PasswordHash = MemberHash,
                    FullName = "Trần Bảo Ngọc",
                    Email = "baongoc@gmail.com",
                    PhoneNumber = "0987654326",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 2, 10, 8, 30, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 22,
                    Username = "member_hoanglong",
                    PasswordHash = MemberHash,
                    FullName = "Lê Hoàng Long",
                    Email = "hoanglong@gmail.com",
                    PhoneNumber = "0987654327",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 3, 5, 9, 15, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 23,
                    Username = "member_mailinh",
                    PasswordHash = MemberHash,
                    FullName = "Đỗ Mai Linh",
                    Email = "mailinh@gmail.com",
                    PhoneNumber = "0987654328",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 4, 1, 14, 20, 0, DateTimeKind.Utc)
                },
                new User
                {
                    UserId = 24,
                    Username = "member_huuduc",
                    PasswordHash = MemberHash,
                    FullName = "Nguyễn Hữu Đức",
                    Email = "huuduc@gmail.com",
                    PhoneNumber = "0987654329",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 1, 5, 10, 0, 0, DateTimeKind.Utc)
                },
                // Thêm 22 Hội viên mới cho các lớp học nhóm
                new User { UserId = 25, Username = "member_minhan", PasswordHash = MemberHash, FullName = "Trịnh Minh An", Email = "minhan@gmail.com", PhoneNumber = "0987654330", Role = "Member", CreatedAt = new DateTime(2026, 2, 12, 8, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 26, Username = "member_khanhlinh", PasswordHash = MemberHash, FullName = "Hoàng Khánh Linh", Email = "khanhlinh@gmail.com", PhoneNumber = "0987654331", Role = "Member", CreatedAt = new DateTime(2026, 2, 18, 9, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 27, Username = "member_tuananh", PasswordHash = MemberHash, FullName = "Đào Tuấn Anh", Email = "tuananh@gmail.com", PhoneNumber = "0987654332", Role = "Member", CreatedAt = new DateTime(2026, 3, 1, 10, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 28, Username = "member_ngocdiep", PasswordHash = MemberHash, FullName = "Phan Ngọc Diệp", Email = "ngocdiep@gmail.com", PhoneNumber = "0987654333", Role = "Member", CreatedAt = new DateTime(2026, 3, 14, 11, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 29, Username = "member_quocbao", PasswordHash = MemberHash, FullName = "Võ Quốc Bảo", Email = "quocbao@gmail.com", PhoneNumber = "0987654334", Role = "Member", CreatedAt = new DateTime(2026, 4, 2, 14, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 30, Username = "member_thuyduong", PasswordHash = MemberHash, FullName = "Lý Thùy Dương", Email = "thuyduong@gmail.com", PhoneNumber = "0987654335", Role = "Member", CreatedAt = new DateTime(2026, 4, 9, 15, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 31, Username = "member_giabao", PasswordHash = MemberHash, FullName = "Bùi Gia Bảo", Email = "giabao@gmail.com", PhoneNumber = "0987654336", Role = "Member", CreatedAt = new DateTime(2026, 5, 11, 8, 30, 0, DateTimeKind.Utc) },
                new User { UserId = 32, Username = "member_hatrang", PasswordHash = MemberHash, FullName = "Ngô Hà Trang", Email = "hatrang@gmail.com", PhoneNumber = "0987654337", Role = "Member", CreatedAt = new DateTime(2026, 5, 19, 9, 30, 0, DateTimeKind.Utc) },
                new User { UserId = 33, Username = "member_trongnghia", PasswordHash = MemberHash, FullName = "Dương Trọng Nghĩa", Email = "trongnghia@gmail.com", PhoneNumber = "0987654338", Role = "Member", CreatedAt = new DateTime(2026, 6, 4, 10, 30, 0, DateTimeKind.Utc) },
                new User { UserId = 34, Username = "member_myduyen", PasswordHash = MemberHash, FullName = "Tạ Mỹ Duyên", Email = "myduyen@gmail.com", PhoneNumber = "0987654339", Role = "Member", CreatedAt = new DateTime(2026, 6, 17, 13, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 35, Username = "member_anhkhoa", PasswordHash = MemberHash, FullName = "Hồ Anh Khoa", Email = "anhkhoa@gmail.com", PhoneNumber = "0987654340", Role = "Member", CreatedAt = new DateTime(2026, 7, 3, 14, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 36, Username = "member_thaovy", PasswordHash = MemberHash, FullName = "Đinh Thảo Vy", Email = "thaovy@gmail.com", PhoneNumber = "0987654341", Role = "Member", CreatedAt = new DateTime(2026, 7, 15, 16, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 37, Username = "member_minhtuan", PasswordHash = MemberHash, FullName = "Cao Minh Tuấn", Email = "minhtuan@gmail.com", PhoneNumber = "0987654342", Role = "Member", CreatedAt = new DateTime(2026, 8, 1, 9, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 38, Username = "member_kimngan", PasswordHash = MemberHash, FullName = "Mai Kim Ngân", Email = "kimngan@gmail.com", PhoneNumber = "0987654343", Role = "Member", CreatedAt = new DateTime(2026, 8, 12, 10, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 39, Username = "member_ductai", PasswordHash = MemberHash, FullName = "Lương Đức Tài", Email = "ductai@gmail.com", PhoneNumber = "0987654344", Role = "Member", CreatedAt = new DateTime(2026, 9, 2, 11, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 40, Username = "member_lanphuong", PasswordHash = MemberHash, FullName = "Châu Lan Phương", Email = "lanphuong@gmail.com", PhoneNumber = "0987654345", Role = "Member", CreatedAt = new DateTime(2026, 9, 10, 15, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 41, Username = "member_quangvinh", PasswordHash = MemberHash, FullName = "Tô Quang Vinh", Email = "quangvinh@gmail.com", PhoneNumber = "0987654346", Role = "Member", CreatedAt = new DateTime(2026, 9, 22, 16, 30, 0, DateTimeKind.Utc) },
                new User { UserId = 42, Username = "member_yennhi", PasswordHash = MemberHash, FullName = "Vương Yến Nhi", Email = "yennhi@gmail.com", PhoneNumber = "0987654347", Role = "Member", CreatedAt = new DateTime(2026, 10, 3, 9, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 43, Username = "member_haiha", PasswordHash = MemberHash, FullName = "Kiều Hải Hà", Email = "haiha@gmail.com", PhoneNumber = "0987654348", Role = "Member", CreatedAt = new DateTime(2026, 10, 11, 10, 30, 0, DateTimeKind.Utc) },
                new User { UserId = 44, Username = "member_tienhung", PasswordHash = MemberHash, FullName = "Chu Tiến Hưng", Email = "tienhung@gmail.com", PhoneNumber = "0987654349", Role = "Member", CreatedAt = new DateTime(2026, 10, 19, 14, 0, 0, DateTimeKind.Utc) },
                new User { UserId = 45, Username = "member_bichthuy", PasswordHash = MemberHash, FullName = "Nghiêm Bích Thủy", Email = "bichthuy@gmail.com", PhoneNumber = "0987654350", Role = "Member", CreatedAt = new DateTime(2026, 11, 1, 8, 45, 0, DateTimeKind.Utc) },
                new User { UserId = 46, Username = "member_hoangphuc", PasswordHash = MemberHash, FullName = "Đoàn Hoàng Phúc", Email = "hoangphuc@gmail.com", PhoneNumber = "0987654351", Role = "Member", CreatedAt = new DateTime(2026, 11, 4, 17, 15, 0, DateTimeKind.Utc) }
            };
        }
    }
}
