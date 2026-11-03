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
            return new List<Member>
            {
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
            };
        }
    }
}
