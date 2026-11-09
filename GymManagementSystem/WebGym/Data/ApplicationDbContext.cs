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

        // ==========================================
        // 1. CÁC BẢNG NỀN TẢNG CƠ BẢN
        // ==========================================
        public DbSet<User> Users => Set<User>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<GymPackage> GymPackages => Set<GymPackage>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Progress> Progress => Set<Progress>();
        public DbSet<Payment> Payments => Set<Payment>();

        // ==========================================
        // 2. CÁC BẢNG CƠ SỞ VẬT CHẤT & CHECK-IN
        // ==========================================
        public DbSet<CheckIn> CheckIns => Set<CheckIn>();
        public DbSet<GymRoom> GymRooms => Set<GymRoom>();
        public DbSet<Equipment> Equipments => Set<Equipment>();
        public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();

        // ==========================================
        // 3. CÁC BẢNG CỬA HÀNG & ĐƠN HÀNG
        // ==========================================
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        // ==========================================
        // 4. LỚP HỌC, THÔNG BÁO & KẾ HOẠCH TẬP LUYỆN
        // ==========================================
        public DbSet<GymClass> GymClasses => Set<GymClass>();
        public DbSet<ClassBooking> ClassBookings => Set<ClassBooking>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // FLUENT API CONFIGURATIONS & RELATIONS
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

            // CheckIns Foreign Key
            modelBuilder.Entity<CheckIn>()
                .HasOne(c => c.Member)
                .WithMany(m => m.CheckIns)
                .HasForeignKey(c => c.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // GymRoom & Equipment / GymClasses (SetNull khi xóa phòng)
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.GymRoom)
                .WithMany(r => r.Equipments)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GymClass>()
                .HasOne(gc => gc.Room)
                .WithMany(r => r.GymClasses)
                .HasForeignKey(gc => gc.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // MaintenanceLogs Foreign Key
            modelBuilder.Entity<MaintenanceLog>()
                .HasOne(m => m.Equipment)
                .WithMany(e => e.MaintenanceLogs)
                .HasForeignKey(m => m.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Products Foreign Key
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Orders Foreign Keys
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Member)
                .WithMany(m => m.Orders)
                .HasForeignKey(o => o.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderDetails Foreign Keys
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // GymClasses Foreign Key (Trainer)
            modelBuilder.Entity<GymClass>()
                .HasOne(gc => gc.Trainer)
                .WithMany(t => t.GymClasses)
                .HasForeignKey(gc => gc.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassBookings Foreign Keys
            modelBuilder.Entity<ClassBooking>()
                .HasOne(cb => cb.GymClass)
                .WithMany(gc => gc.ClassBookings)
                .HasForeignKey(cb => cb.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassBooking>()
                .HasOne(cb => cb.Member)
                .WithMany(m => m.ClassBookings)
                .HasForeignKey(cb => cb.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notifications Foreign Key
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // WorkoutPlans Foreign Keys
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(wp => wp.Member)
                .WithMany(m => m.WorkoutPlans)
                .HasForeignKey(wp => wp.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(wp => wp.Trainer)
                .WithMany(t => t.WorkoutPlans)
                .HasForeignKey(wp => wp.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================================
            // SEED DATA - TỔ CHỨC THEO TỪNG BẢNG
            // ==========================================
            modelBuilder.SeedUsers();
            modelBuilder.SeedTrainers();
            modelBuilder.SeedMembers();
            modelBuilder.SeedGymPackages();
            modelBuilder.SeedSubscriptions();
            modelBuilder.SeedPayments();
            modelBuilder.SeedProgress();
            modelBuilder.SeedBookings();

            // Dữ liệu mẫu cho các bảng mới
            modelBuilder.SeedGymRooms();
            modelBuilder.SeedEquipments();
            modelBuilder.SeedMaintenanceLogs();
            modelBuilder.SeedCheckIns();
            modelBuilder.SeedProductCategories();
            modelBuilder.SeedProducts();
            modelBuilder.SeedOrders();
            modelBuilder.SeedOrderDetails();
            modelBuilder.SeedGymClasses();
            modelBuilder.SeedClassBookings();
            modelBuilder.SeedNotifications();
            modelBuilder.SeedWorkoutPlans();
        }
    }
}
