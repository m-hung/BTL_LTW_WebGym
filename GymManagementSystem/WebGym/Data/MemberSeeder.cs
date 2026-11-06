using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class MemberSeeder
    {
        public static void SeedMembers(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasData(GetData());
        }

        public static List<Member> GetData()
        {
            // Tài khoản mới tạo / chưa đo InBody sẽ không có chỉ số (để trống / null theo yêu cầu)
            return new List<Member>
            {
                new Member
                {
                    MemberId = 1,
                    UserId = 5,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1997, 4, 12),
                    Height = null,
                    Weight = null,
                    BMR = null,
                    BodyFatPercentage = null,
                    MuscleMass = null,
                    Chest = null,
                    Waist = null,
                    Hips = null
                },
                new Member
                {
                    MemberId = 2,
                    UserId = 6,
                    Gender = "Nữ",
                    DateOfBirth = new DateTime(2001, 8, 20),
                    Height = null,
                    Weight = null,
                    BMR = null,
                    BodyFatPercentage = null,
                    MuscleMass = null,
                    Chest = null,
                    Waist = null,
                    Hips = null
                },
                new Member
                {
                    MemberId = 3,
                    UserId = 7,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1995, 11, 5),
                    Height = null,
                    Weight = null,
                    BMR = null,
                    BodyFatPercentage = null,
                    MuscleMass = null,
                    Chest = null,
                    Waist = null,
                    Hips = null
                },
                new Member
                {
                    MemberId = 4,
                    UserId = 8,
                    Gender = "Nữ",
                    DateOfBirth = new DateTime(1999, 2, 14),
                    Height = null,
                    Weight = null,
                    BMR = null,
                    BodyFatPercentage = null,
                    MuscleMass = null,
                    Chest = null,
                    Waist = null,
                    Hips = null
                },
                new Member
                {
                    MemberId = 5,
                    UserId = 9,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1992, 6, 30),
                    Height = null,
                    Weight = null,
                    BMR = null,
                    BodyFatPercentage = null,
                    MuscleMass = null,
                    Chest = null,
                    Waist = null,
                    Hips = null
                }
            };
        }
    }
}
