using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<GymPackage> GymPackages => Set<GymPackage>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Progress> Progress => Set<Progress>();
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. FLUENT API CONFIGURATIONS & RELATIONS
            // ==========================================

            // User - Member (1 - 1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Member)
                .WithOne(m => m.User)
                .HasForeignKey<Member>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Trainer (1 - 1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Trainer)
                .WithOne(t => t.User)
                .HasForeignKey<Trainer>(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique Indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Subscriptions Foreign Keys
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Member)
                .WithMany(m => m.Subscriptions)
                .HasForeignKey(s => s.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Package)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(s => s.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bookings Foreign Keys
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Trainer)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Progress Foreign Key
            modelBuilder.Entity<Progress>()
                .HasOne(p => p.Member)
                .WithMany(m => m.ProgressRecords)
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Payments Foreign Keys
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Member)
                .WithMany(m => m.Payments)
                .HasForeignKey(p => p.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Package)
                .WithMany(g => g.Payments)
                .HasForeignKey(p => p.PackageId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================================
            // 2. SEED DATA - CHUẨN ĐỂ MIGRATION & DEMO
            // ==========================================

            // Passwords hash (SHA-256 base64)
            // Admin@123   -> 6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=
            // PT@123      -> T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=
            // Member@123  -> q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=
            const string adminHash = "6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=";
            const string ptHash = "T6jis6FX1pX02Z7bA4WYKdv2Yv//nDSnSZ6x4/S/Z44=";
            const string memberHash = "q+LT7VQZ4aIpPANKazdaYi/1pg5aww8pxGEiCJj/3Zc=";

            // 2.1 Seed Users (1 Admin, 3 PT, 5 Member)
            modelBuilder.Entity<User>().HasData(
                // Admin
                new User
                {
                    UserId = 1,
                    Username = "admin",
                    PasswordHash = adminHash,
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
                    PasswordHash = ptHash,
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
                    PasswordHash = ptHash,
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
                    PasswordHash = ptHash,
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
                    PasswordHash = memberHash,
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
                    PasswordHash = memberHash,
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
                    PasswordHash = memberHash,
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
                    PasswordHash = memberHash,
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
                    PasswordHash = memberHash,
                    FullName = "Phạm Hoàng Nam",
                    Email = "hoangnam@gmail.com",
                    PhoneNumber = "0987654325",
                    Role = "Member",
                    CreatedAt = new DateTime(2026, 7, 20, 16, 45, 0, DateTimeKind.Utc)
                }
            );

            // 2.2 Seed Trainers (3 PTs)
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer
                {
                    TrainerId = 1,
                    UserId = 2,
                    Specialization = "Tăng cơ, Giảm mỡ nâng cao & Thể hình chuyên nghiệp (Bodybuilding)",
                    Bio = "HLV 6 năm kinh nghiệm, chứng chỉ quốc tế NASM, từng huấn luyện nhiều vận động viên thể hình.",
                    AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=300"
                },
                new Trainer
                {
                    TrainerId = 2,
                    UserId = 3,
                    Specialization = "Cardio cường độ cao (HIIT), Thể lực & Sức bền (Functional Training)",
                    Bio = "HLV 4 năm kinh nghiệm, chuyên về giảm cân cấp tốc, rèn luyện tim mạch và cải thiện chuyển hóa.",
                    AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300"
                },
                new Trainer
                {
                    TrainerId = 3,
                    UserId = 4,
                    Specialization = "Pilates, Yoga & Điều chỉnh tư thế cột sống (Rehabilitation)",
                    Bio = "Chuyên gia phục hồi tư thế, tăng độ dẻo dai và giảm đau mỏi lưng cổ vai gáy cho dân văn phòng.",
                    AvatarUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=300"
                }
            );

            // 2.3 Seed Members (5 Members với chỉ số chi tiết)
            modelBuilder.Entity<Member>().HasData(
                new Member
                {
                    MemberId = 1,
                    UserId = 5,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1997, 4, 12),
                    Height = 170.0,
                    Weight = 78.5,
                    BMR = 1720.0,
                    BodyFatPercentage = 25.5,
                    MuscleMass = 32.0,
                    Chest = 98.0,
                    Waist = 88.0,
                    Hips = 96.0
                },
                new Member
                {
                    MemberId = 2,
                    UserId = 6,
                    Gender = "Nữ",
                    DateOfBirth = new DateTime(2001, 8, 20),
                    Height = 162.0,
                    Weight = 56.0,
                    BMR = 1350.0,
                    BodyFatPercentage = 24.0,
                    MuscleMass = 22.5,
                    Chest = 84.0,
                    Waist = 68.0,
                    Hips = 91.0
                },
                new Member
                {
                    MemberId = 3,
                    UserId = 7,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1995, 11, 5),
                    Height = 176.0,
                    Weight = 84.0,
                    BMR = 1850.0,
                    BodyFatPercentage = 26.0,
                    MuscleMass = 34.5,
                    Chest = 102.0,
                    Waist = 92.0,
                    Hips = 100.0
                },
                new Member
                {
                    MemberId = 4,
                    UserId = 8,
                    Gender = "Nữ",
                    DateOfBirth = new DateTime(1999, 2, 14),
                    Height = 158.0,
                    Weight = 49.0,
                    BMR = 1280.0,
                    BodyFatPercentage = 21.5,
                    MuscleMass = 20.0,
                    Chest = 82.0,
                    Waist = 64.0,
                    Hips = 88.0
                },
                new Member
                {
                    MemberId = 5,
                    UserId = 9,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1992, 6, 30),
                    Height = 174.0,
                    Weight = 73.0,
                    BMR = 1690.0,
                    BodyFatPercentage = 19.0,
                    MuscleMass = 33.0,
                    Chest = 96.0,
                    Waist = 80.0,
                    Hips = 94.0
                }
            );

            // 2.4 Seed GymPackages (Đúng 4 gói tập yêu cầu)
            modelBuilder.Entity<GymPackage>().HasData(
                new GymPackage
                {
                    PackageId = 1,
                    PackageName = "Gói Đồng (Basic 1 Tháng)",
                    Description = "Gói tập cơ bản 30 ngày, sử dụng toàn bộ trang thiết bị Gym hiện đại, tủ locker và phòng tắm nước nóng.",
                    Price = 500000m,
                    DurationDays = 30,
                    IncludesPT = false
                },
                new GymPackage
                {
                    PackageId = 2,
                    PackageName = "Gói Bạc (Standard 3 Tháng)",
                    Description = "Gói tập 90 ngày tiết kiệm, không giới hạn khung giờ tập, miễn phí đo chỉ số cơ thể định kỳ (InBody).",
                    Price = 1300000m,
                    DurationDays = 90,
                    IncludesPT = false
                },
                new GymPackage
                {
                    PackageId = 3,
                    PackageName = "Gói Vàng (VIP 6 Tháng kèm PT)",
                    Description = "Gói tập 180 ngày cao cấp, tập kèm PT không giới hạn, sử dụng phòng xông hơi Sauna & bể bơi nước ấm.",
                    Price = 4500000m,
                    DurationDays = 180,
                    IncludesPT = true
                },
                new GymPackage
                {
                    PackageId = 4,
                    PackageName = "Gói Kim Cương (VVIP 1 Năm kèm PT)",
                    Description = "Gói tập trọn gói 365 ngày đặc quyền tối cao, tập kèm PT không giới hạn, tư vấn dinh dưỡng AI và khăn nước riêng.",
                    Price = 8000000m,
                    DurationDays = 365,
                    IncludesPT = true
                }
            );

            // 2.5 Seed Subscriptions (Rải rác 6 tháng gần nhất)
            modelBuilder.Entity<Subscription>().HasData(
                new Subscription
                {
                    SubscriptionId = 1,
                    MemberId = 3, // Thắng
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 8, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 2,
                    MemberId = 2, // Hương
                    PackageId = 4, // Gói Kim Cương 8tr
                    OriginalPrice = 8000000m,
                    PaidAmount = 8000000m,
                    StartDate = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 3,
                    MemberId = 5, // Nam
                    PackageId = 1, // Gói Đồng 500k
                    OriginalPrice = 500000m,
                    PaidAmount = 500000m,
                    StartDate = new DateTime(2026, 7, 20, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 19, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 4,
                    MemberId = 1, // Hải
                    PackageId = 3, // Gói Vàng 4.5tr kèm PT
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 5,
                    MemberId = 3, // Thắng gia hạn VIP
                    PackageId = 3, // Gói Vàng 4.5tr
                    OriginalPrice = 4500000m,
                    PaidAmount = 4500000m,
                    StartDate = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 2, 6, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 6,
                    MemberId = 5, // Nam nâng cấp Kim Cương
                    PackageId = 4, // Gói Kim Cương 8tr
                    OriginalPrice = 8000000m,
                    PaidAmount = 8000000m,
                    StartDate = new DateTime(2026, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                },
                new Subscription
                {
                    SubscriptionId = 7,
                    MemberId = 4, // Phương Anh
                    PackageId = 1, // Gói Đồng 500k
                    OriginalPrice = 500000m,
                    PaidAmount = 500000m,
                    StartDate = new DateTime(2026, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Expired"
                },
                new Subscription
                {
                    SubscriptionId = 8,
                    MemberId = 4, // Phương Anh gia hạn Gói Bạc
                    PackageId = 2, // Gói Bạc 1.3tr
                    OriginalPrice = 1300000m,
                    PaidAmount = 1300000m,
                    StartDate = new DateTime(2026, 10, 6, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 4, 0, 0, 0, DateTimeKind.Utc),
                    Status = "Active"
                }
            );

            // 2.6 Seed Payments (Đa dạng doanh thu 500k, 1.3tr, 4.5tr, 8tr, Momo, VNPay, BankTransfer)
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    PaymentId = 1,
                    MemberId = 3,
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "Momo",
                    TransactionCode = "MOM20260510084512",
                    PaymentDate = new DateTime(2026, 5, 10, 8, 45, 12, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 2,
                    MemberId = 2,
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "BankTransfer",
                    TransactionCode = "MBB20260615143011",
                    PaymentDate = new DateTime(2026, 6, 15, 14, 30, 11, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 3,
                    MemberId = 5,
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260720173000",
                    PaymentDate = new DateTime(2026, 7, 20, 17, 30, 0, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 4,
                    MemberId = 1,
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260801091523",
                    PaymentDate = new DateTime(2026, 8, 1, 9, 15, 23, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 5,
                    MemberId = 3,
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20260810102045",
                    PaymentDate = new DateTime(2026, 8, 10, 10, 20, 45, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 6,
                    MemberId = 5,
                    PackageId = 4,
                    Amount = 8000000m,
                    PaymentMethod = "BankTransfer",
                    TransactionCode = "TCB20260901091122",
                    PaymentDate = new DateTime(2026, 9, 1, 9, 11, 22, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 7,
                    MemberId = 4,
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Momo",
                    TransactionCode = "MOM20260905161209",
                    PaymentDate = new DateTime(2026, 9, 5, 16, 12, 9, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 8,
                    MemberId = 4,
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "BankTransfer",
                    TransactionCode = "VCB20261006114530",
                    PaymentDate = new DateTime(2026, 10, 6, 11, 45, 30, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 9,
                    MemberId = 1,
                    PackageId = 1,
                    Amount = 500000m,
                    PaymentMethod = "Momo",
                    TransactionCode = "MOM20261025190011",
                    PaymentDate = new DateTime(2026, 10, 25, 19, 0, 11, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 10,
                    MemberId = 2,
                    PackageId = 2,
                    Amount = 1300000m,
                    PaymentMethod = "VNPay",
                    TransactionCode = "VNP20261101083015",
                    PaymentDate = new DateTime(2026, 11, 1, 8, 30, 15, DateTimeKind.Utc),
                    Status = "Success"
                },
                new Payment
                {
                    PaymentId = 11,
                    MemberId = 3,
                    PackageId = 3,
                    Amount = 4500000m,
                    PaymentMethod = "Momo",
                    TransactionCode = "MOM20261015112233",
                    PaymentDate = new DateTime(2026, 10, 15, 11, 22, 33, DateTimeKind.Utc),
                    Status = "Failed"
                }
            );

            // 2.7 Seed Progress (6 bản ghi của MemberId = 1 - Chỉ số cải thiện ấn tượng)
            modelBuilder.Entity<Progress>().HasData(
                new Progress
                {
                    ProgressId = 1,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 8, 1, 8, 30, 0, DateTimeKind.Utc),
                    Weight = 78.5,
                    BodyFatPercentage = 25.5,
                    MuscleMass = 32.0,
                    Chest = 98.0,
                    Waist = 88.0,
                    Hips = 96.0,
                    Notes = "Đo InBody ngày đầu nhập môn. Thể trạng nhiều mỡ nội tạng, cơ địa dễ tích mỡ bụng."
                },
                new Progress
                {
                    ProgressId = 2,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 8, 20, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 76.8,
                    BodyFatPercentage = 24.2,
                    MuscleMass = 32.4,
                    Chest = 98.5,
                    Waist = 85.5,
                    Hips = 95.0,
                    Notes = "Thích nghi tốt với bài tập kháng lực và cardio HIIT. Bắt đầu quen với thực đơn thâm hụt calo."
                },
                new Progress
                {
                    ProgressId = 3,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 9, 10, 8, 45, 0, DateTimeKind.Utc),
                    Weight = 74.9,
                    BodyFatPercentage = 22.8,
                    MuscleMass = 32.9,
                    Chest = 99.0,
                    Waist = 83.0,
                    Hips = 94.0,
                    Notes = "Vòng eo giảm rõ rệt 5cm, cơ ngực săn chắc, sức bền tim mạch tăng đáng kể."
                },
                new Progress
                {
                    ProgressId = 4,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 9, 30, 9, 15, 0, DateTimeKind.Utc),
                    Weight = 73.2,
                    BodyFatPercentage = 21.2,
                    MuscleMass = 33.5,
                    Chest = 99.5,
                    Waist = 81.0,
                    Hips = 93.5,
                    Notes = "Tăng cơ nạc và siết mỡ liên tục. Mức tạ Squat và Bench Press tăng thêm 15kg."
                },
                new Progress
                {
                    ProgressId = 5,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 10, 18, 8, 30, 0, DateTimeKind.Utc),
                    Weight = 71.6,
                    BodyFatPercentage = 19.8,
                    MuscleMass = 34.0,
                    Chest = 100.0,
                    Waist = 79.0,
                    Hips = 93.0,
                    Notes = "Tỷ lệ mỡ giảm dưới mốc 20%, cơ bụng lộ 4 múi rõ nét, phong độ tập luyện rất cao."
                },
                new Progress
                {
                    ProgressId = 6,
                    MemberId = 1,
                    RecordedDate = new DateTime(2026, 11, 1, 9, 0, 0, DateTimeKind.Utc),
                    Weight = 70.2,
                    BodyFatPercentage = 18.5,
                    MuscleMass = 34.6,
                    Chest = 101.0,
                    Waist = 77.5,
                    Hips = 92.5,
                    Notes = "Cột mốc 3 tháng: Giảm 8.3kg mỡ, tăng 2.6kg cơ, vòng eo giảm 10.5cm. Chỉ số xuất sắc!"
                }
            );

            // 2.8 Seed Bookings (10 lịch tập của TrainerId = 1 với đầy đủ trạng thái)
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    MemberId = 1,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 26, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "08:00 - 09:30",
                    Status = "Completed",
                    Notes = "Tập ngực & tay sau (Chest & Triceps focus)"
                },
                new Booking
                {
                    BookingId = 2,
                    MemberId = 2,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 27, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:30 - 19:00",
                    Status = "Completed",
                    Notes = "Cardio HIIT đốt mỡ & giãn cơ phục hồi"
                },
                new Booking
                {
                    BookingId = 3,
                    MemberId = 3,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 28, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "09:00 - 10:30",
                    Status = "Completed",
                    Notes = "Chân mông đùi (Leg day - Squat form check)"
                },
                new Booking
                {
                    BookingId = 4,
                    MemberId = 1,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 29, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "08:00 - 09:30",
                    Status = "Completed",
                    Notes = "Lưng xô & tay trước (Back & Biceps)"
                },
                new Booking
                {
                    BookingId = 5,
                    MemberId = 5,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 10, 30, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Cancelled",
                    Notes = "Hội viên có việc bận đột xuất xin hoãn ca"
                },
                new Booking
                {
                    BookingId = 6,
                    MemberId = 1,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 2, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "08:00 - 09:30",
                    Status = "Approved",
                    Notes = "Đo lại chỉ số đầu tháng & tập bài đa khớp compound"
                },
                new Booking
                {
                    BookingId = 7,
                    MemberId = 2,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 3, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "17:30 - 19:00",
                    Status = "Approved",
                    Notes = "Tập mông đùi Glutes hypertrophy"
                },
                new Booking
                {
                    BookingId = 8,
                    MemberId = 4,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 4, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "15:00 - 16:30",
                    Status = "Pending",
                    Notes = "Hội viên mới đăng ký buổi đầu thẩm định thể lực"
                },
                new Booking
                {
                    BookingId = 9,
                    MemberId = 3,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "10:00 - 11:30",
                    Status = "Pending",
                    Notes = "Tập vai & cơ bụng core strength"
                },
                new Booking
                {
                    BookingId = 10,
                    MemberId = 5,
                    TrainerId = 1,
                    BookingDate = new DateTime(2026, 11, 6, 0, 0, 0, DateTimeKind.Utc),
                    TimeSlot = "18:00 - 19:30",
                    Status = "Pending",
                    Notes = "Đăng ký buổi tập bù ca tuần trước"
                }
            );
        }
    }
}
