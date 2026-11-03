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
            // 2. SEED DATA - TỔ CHỨC THEO CÁC FILE RIÊNG BIỆT TỪNG BẢNG
            // ==========================================
            modelBuilder.SeedUsers();
            modelBuilder.SeedTrainers();
            modelBuilder.SeedMembers();
            modelBuilder.SeedGymPackages();
            modelBuilder.SeedSubscriptions();
            modelBuilder.SeedPayments();
            modelBuilder.SeedProgress();
            modelBuilder.SeedBookings();
        }
    }
}
