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
                    AvatarUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400",
                    CardCode = "GYM-MB-0001",
                    QrCode = "GYM-MB-0001",
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
                    AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400",
                    CardCode = "GYM-MB-0002",
                    QrCode = "GYM-MB-0002",
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
                    AvatarUrl = "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=400",
                    CardCode = "GYM-MB-0003",
                    QrCode = "GYM-MB-0003",
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
                    AvatarUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400",
                    CardCode = "GYM-MB-0004",
                    QrCode = "GYM-MB-0004",
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
                    AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400",
                    CardCode = "GYM-MB-0005",
                    QrCode = "GYM-MB-0005",
                    Height = 174.0,
                    Weight = 73.0,
                    BMR = 1690.0,
                    BodyFatPercentage = 19.0,
                    MuscleMass = 33.0,
                    Chest = 96.0,
                    Waist = 80.0,
                    Hips = 94.0
                },
                new Member
                {
                    MemberId = 21,
                    UserId = 21,
                    Gender = "Nữ",
                    DateOfBirth = new DateTime(2000, 3, 12),
                    AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400",
                    CardCode = "GYM-MB-0021",
                    QrCode = "GYM-MB-0021",
                    Height = 165.0,
                    Weight = 54.0,
                    BMR = 1320.0,
                    BodyFatPercentage = 23.5,
                    MuscleMass = 21.8,
                    Chest = 85.0,
                    Waist = 65.0,
                    Hips = 90.0
                },
                new Member
                {
                    MemberId = 22,
                    UserId = 22,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1996, 7, 25),
                    AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400",
                    CardCode = "GYM-MB-0022",
                    QrCode = "GYM-MB-0022",
                    Height = 178.0,
                    Weight = 82.0,
                    BMR = 1810.0,
                    BodyFatPercentage = 24.0,
                    MuscleMass = 35.0,
                    Chest = 101.0,
                    Waist = 86.0,
                    Hips = 98.0
                },
                new Member
                {
                    MemberId = 23,
                    UserId = 23,
                    Gender = "Nữ",
                    DateOfBirth = new DateTime(2002, 11, 18),
                    AvatarUrl = "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400",
                    CardCode = "GYM-MB-0023",
                    QrCode = "GYM-MB-0023",
                    Height = 160.0,
                    Weight = 52.5,
                    BMR = 1290.0,
                    BodyFatPercentage = 22.0,
                    MuscleMass = 20.5,
                    Chest = 83.0,
                    Waist = 63.0,
                    Hips = 89.0
                },
                new Member
                {
                    MemberId = 24,
                    UserId = 24,
                    Gender = "Nam",
                    DateOfBirth = new DateTime(1994, 9, 8),
                    AvatarUrl = "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=400",
                    CardCode = "GYM-MB-0024",
                    QrCode = "GYM-MB-0024",
                    Height = 172.0,
                    Weight = 75.0,
                    BMR = 1700.0,
                    BodyFatPercentage = 21.0,
                    MuscleMass = 33.0,
                    Chest = 97.0,
                    Waist = 82.0,
                    Hips = 95.0
                },
                // 22 Hội viên mới
                new Member { MemberId = 25, UserId = 25, Gender = "Nam", DateOfBirth = new DateTime(1997, 3, 12), AvatarUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", CardCode = "GYM-MB-0025", QrCode = "GYM-MB-0025", Height = 175.0, Weight = 72.0, BMR = 1690.0, BodyFatPercentage = 18.5, MuscleMass = 33.2, Chest = 96.0, Waist = 79.0, Hips = 94.0 },
                new Member { MemberId = 26, UserId = 26, Gender = "Nữ", DateOfBirth = new DateTime(1999, 6, 21), AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", CardCode = "GYM-MB-0026", QrCode = "GYM-MB-0026", Height = 163.0, Weight = 53.0, BMR = 1310.0, BodyFatPercentage = 22.0, MuscleMass = 21.5, Chest = 84.0, Waist = 64.0, Hips = 91.0 },
                new Member { MemberId = 27, UserId = 27, Gender = "Nam", DateOfBirth = new DateTime(1995, 11, 3), AvatarUrl = "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=400", CardCode = "GYM-MB-0027", QrCode = "GYM-MB-0027", Height = 179.0, Weight = 80.5, BMR = 1800.0, BodyFatPercentage = 20.0, MuscleMass = 36.0, Chest = 102.0, Waist = 84.0, Hips = 98.0 },
                new Member { MemberId = 28, UserId = 28, Gender = "Nữ", DateOfBirth = new DateTime(2000, 1, 17), AvatarUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400", CardCode = "GYM-MB-0028", QrCode = "GYM-MB-0028", Height = 161.0, Weight = 51.0, BMR = 1280.0, BodyFatPercentage = 21.5, MuscleMass = 20.2, Chest = 82.0, Waist = 62.0, Hips = 89.0 },
                new Member { MemberId = 29, UserId = 29, Gender = "Nam", DateOfBirth = new DateTime(1993, 8, 29), AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400", CardCode = "GYM-MB-0029", QrCode = "GYM-MB-0029", Height = 174.0, Weight = 76.0, BMR = 1720.0, BodyFatPercentage = 21.0, MuscleMass = 33.8, Chest = 98.0, Waist = 83.0, Hips = 96.0 },
                new Member { MemberId = 30, UserId = 30, Gender = "Nữ", DateOfBirth = new DateTime(1998, 4, 14), AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400", CardCode = "GYM-MB-0030", QrCode = "GYM-MB-0030", Height = 165.0, Weight = 55.0, BMR = 1350.0, BodyFatPercentage = 23.0, MuscleMass = 22.1, Chest = 86.0, Waist = 65.0, Hips = 92.0 },
                new Member { MemberId = 31, UserId = 31, Gender = "Nam", DateOfBirth = new DateTime(2001, 2, 7), AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400", CardCode = "GYM-MB-0031", QrCode = "GYM-MB-0031", Height = 177.0, Weight = 73.5, BMR = 1740.0, BodyFatPercentage = 17.5, MuscleMass = 34.5, Chest = 97.0, Waist = 78.0, Hips = 95.0 },
                new Member { MemberId = 32, UserId = 32, Gender = "Nữ", DateOfBirth = new DateTime(1996, 9, 25), AvatarUrl = "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400", CardCode = "GYM-MB-0032", QrCode = "GYM-MB-0032", Height = 160.0, Weight = 50.5, BMR = 1270.0, BodyFatPercentage = 21.0, MuscleMass = 20.4, Chest = 83.0, Waist = 61.5, Hips = 88.5 },
                new Member { MemberId = 33, UserId = 33, Gender = "Nam", DateOfBirth = new DateTime(1992, 12, 19), AvatarUrl = "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=400", CardCode = "GYM-MB-0033", QrCode = "GYM-MB-0033", Height = 180.0, Weight = 84.0, BMR = 1840.0, BodyFatPercentage = 22.5, MuscleMass = 36.8, Chest = 104.0, Waist = 87.0, Hips = 99.0 },
                new Member { MemberId = 34, UserId = 34, Gender = "Nữ", DateOfBirth = new DateTime(2001, 5, 30), AvatarUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=400", CardCode = "GYM-MB-0034", QrCode = "GYM-MB-0034", Height = 164.0, Weight = 54.0, BMR = 1330.0, BodyFatPercentage = 22.5, MuscleMass = 21.8, Chest = 85.0, Waist = 64.0, Hips = 91.0 },
                new Member { MemberId = 35, UserId = 35, Gender = "Nam", DateOfBirth = new DateTime(1998, 7, 11), AvatarUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", CardCode = "GYM-MB-0035", QrCode = "GYM-MB-0035", Height = 173.0, Weight = 71.0, BMR = 1680.0, BodyFatPercentage = 18.0, MuscleMass = 32.9, Chest = 95.0, Waist = 79.0, Hips = 94.0 },
                new Member { MemberId = 36, UserId = 36, Gender = "Nữ", DateOfBirth = new DateTime(2002, 8, 4), AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", CardCode = "GYM-MB-0036", QrCode = "GYM-MB-0036", Height = 159.0, Weight = 49.0, BMR = 1250.0, BodyFatPercentage = 20.5, MuscleMass = 19.8, Chest = 81.0, Waist = 60.0, Hips = 87.0 },
                new Member { MemberId = 37, UserId = 37, Gender = "Nam", DateOfBirth = new DateTime(1994, 4, 22), AvatarUrl = "https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=400", CardCode = "GYM-MB-0037", QrCode = "GYM-MB-0037", Height = 176.0, Weight = 77.5, BMR = 1760.0, BodyFatPercentage = 19.5, MuscleMass = 35.1, Chest = 100.0, Waist = 82.0, Hips = 96.0 },
                new Member { MemberId = 38, UserId = 38, Gender = "Nữ", DateOfBirth = new DateTime(1997, 10, 8), AvatarUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400", CardCode = "GYM-MB-0038", QrCode = "GYM-MB-0038", Height = 162.0, Weight = 52.0, BMR = 1300.0, BodyFatPercentage = 21.8, MuscleMass = 21.0, Chest = 84.0, Waist = 63.0, Hips = 90.0 },
                new Member { MemberId = 39, UserId = 39, Gender = "Nam", DateOfBirth = new DateTime(1996, 1, 15), AvatarUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400", CardCode = "GYM-MB-0039", QrCode = "GYM-MB-0039", Height = 178.0, Weight = 79.0, BMR = 1785.0, BodyFatPercentage = 19.0, MuscleMass = 35.9, Chest = 101.0, Waist = 83.0, Hips = 97.0 },
                new Member { MemberId = 40, UserId = 40, Gender = "Nữ", DateOfBirth = new DateTime(1999, 12, 2), AvatarUrl = "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400", CardCode = "GYM-MB-0040", QrCode = "GYM-MB-0040", Height = 166.0, Weight = 56.5, BMR = 1370.0, BodyFatPercentage = 23.2, MuscleMass = 22.6, Chest = 87.0, Waist = 66.0, Hips = 93.0 },
                new Member { MemberId = 41, UserId = 41, Gender = "Nam", DateOfBirth = new DateTime(1995, 5, 19), AvatarUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400", CardCode = "GYM-MB-0041", QrCode = "GYM-MB-0041", Height = 175.0, Weight = 74.5, BMR = 1715.0, BodyFatPercentage = 18.8, MuscleMass = 34.0, Chest = 98.0, Waist = 80.0, Hips = 95.0 },
                new Member { MemberId = 42, UserId = 42, Gender = "Nữ", DateOfBirth = new DateTime(2000, 7, 27), AvatarUrl = "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400", CardCode = "GYM-MB-0042", QrCode = "GYM-MB-0042", Height = 161.0, Weight = 50.0, BMR = 1275.0, BodyFatPercentage = 20.8, MuscleMass = 20.3, Chest = 82.0, Waist = 61.0, Hips = 89.0 },
                new Member { MemberId = 43, UserId = 43, Gender = "Nữ", DateOfBirth = new DateTime(1998, 11, 14), AvatarUrl = "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=400", CardCode = "GYM-MB-0043", QrCode = "GYM-MB-0043", Height = 163.0, Weight = 54.0, BMR = 1320.0, BodyFatPercentage = 22.4, MuscleMass = 21.6, Chest = 85.0, Waist = 64.0, Hips = 91.0 },
                new Member { MemberId = 44, UserId = 44, Gender = "Nam", DateOfBirth = new DateTime(1991, 9, 6), AvatarUrl = "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?w=400", CardCode = "GYM-MB-0044", QrCode = "GYM-MB-0044", Height = 177.0, Weight = 81.0, BMR = 1790.0, BodyFatPercentage = 22.0, MuscleMass = 35.6, Chest = 102.0, Waist = 85.0, Hips = 98.0 },
                new Member { MemberId = 45, UserId = 45, Gender = "Nữ", DateOfBirth = new DateTime(1997, 3, 28), AvatarUrl = "https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=400", CardCode = "GYM-MB-0045", QrCode = "GYM-MB-0045", Height = 162.0, Weight = 52.5, BMR = 1305.0, BodyFatPercentage = 21.9, MuscleMass = 21.2, Chest = 84.0, Waist = 63.0, Hips = 90.0 },
                new Member { MemberId = 46, UserId = 46, Gender = "Nam", DateOfBirth = new DateTime(1999, 6, 10), AvatarUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=400", CardCode = "GYM-MB-0046", QrCode = "GYM-MB-0046", Height = 174.0, Weight = 70.0, BMR = 1675.0, BodyFatPercentage = 16.5, MuscleMass = 33.4, Chest = 95.0, Waist = 77.0, Hips = 93.0 }
            };
        }
    }
}
