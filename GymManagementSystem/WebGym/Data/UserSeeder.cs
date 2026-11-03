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
                    CreatedAt = new DateTime(2026, 8, 1, 8, 0, 0, DateTimeKind.Utc)
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
                }
            };
        }
    }
}
