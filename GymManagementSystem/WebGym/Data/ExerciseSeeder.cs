using Microsoft.EntityFrameworkCore;
using WebGym.Models.Entities;

namespace WebGym.Data
{
    /// <summary>
    /// Danh mục bài tập cho giáo án AI. Mỗi bài gắn với một thiết bị trong EquipmentSeeder (EquipmentId 1-10),
    /// hoặc không cần thiết bị (EquipmentId = null). AI chỉ được chọn bài trong danh sách này.
    /// Lưu ý: chỉ dùng chuỗi một dòng, không dùng chuỗi nhiều dòng @"..." để dữ liệu mẫu giống nhau trên mọi máy.
    /// </summary>
    public static class ExerciseSeeder
    {
        public static void SeedExercises(this ModelBuilder modelBuilder) => Seed(modelBuilder);

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>().HasData(GetData());
        }

        public static List<Exercise> GetData()
        {
            return new List<Exercise>
            {
                new Exercise { ExerciseId = 1, Name = "Đẩy ngực tạ đòn (Barbell Bench Press)", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "shoulder", DefaultRestSec = 120,
                    HowTo = "Nằm ghế phẳng, hạ tạ chạm giữa ngực, khuỷu tay mở khoảng 45 độ, đẩy thẳng lên. Luôn tập có người hỗ trợ hoặc chốt an toàn của Power Rack." },
                new Exercise { ExerciseId = 2, Name = "Đẩy ngực trên Smith Machine", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 5, AvoidIfInjury = "shoulder", DefaultRestSec = 90,
                    HowTo = "Đặt ghế dưới thanh Smith, hạ thanh chậm tới ngang ngực rồi đẩy lên. Thanh chạy theo ray nên an toàn cho người mới." },
                new Exercise { ExerciseId = 3, Name = "Đẩy ngực tạ đơn (Dumbbell Bench Press)", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "shoulder", DefaultRestSec = 90,
                    HowTo = "Nằm ghế phẳng, hai tạ ngang ngực, đẩy lên và khép nhẹ ở đỉnh. Hạ chậm 2-3 giây, không thả rơi tạ." },
                new Exercise { ExerciseId = 4, Name = "Đẩy ngực dốc tạ đơn (Incline Dumbbell Press)", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "shoulder", DefaultRestSec = 90,
                    HowTo = "Ghế dốc 30-45 độ, đẩy tạ từ ngực trên lên cao. Giữ lưng áp sát ghế, không ưỡn quá mức." },
                new Exercise { ExerciseId = 5, Name = "Ép ngực trên máy (Pec Fly)", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 8, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Ngồi thẳng lưng, tay hơi cong, ép hai tay vào trước ngực và giữ 1 giây rồi mở ra chậm." },
                new Exercise { ExerciseId = 6, Name = "Ép ngực cáp chéo (Cable Crossover)", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đứng giữa giàn cáp, bước một chân lên trước, kéo hai tay chéo xuống trước bụng, cảm nhận ngực siết lại." },
                new Exercise { ExerciseId = 7, Name = "Hít đất (Push-up)", MuscleGroup = "Ngực", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = null, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Hai tay rộng hơn vai, thân thẳng từ đầu tới gót. Người mới có thể chống gối. Hạ ngực gần sàn rồi đẩy lên." },
                new Exercise { ExerciseId = 8, Name = "Đẩy vai tạ đơn ngồi (Seated Dumbbell Press)", MuscleGroup = "Vai", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "shoulder", DefaultRestSec = 90,
                    HowTo = "Ngồi ghế dựng thẳng, đẩy hai tạ từ ngang tai lên đỉnh đầu, không khoá cứng khuỷu tay." },
                new Exercise { ExerciseId = 9, Name = "Đẩy vai tạ đòn đứng (Overhead Press)", MuscleGroup = "Vai", Category = ExerciseCategories.Push, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "shoulder,back", DefaultRestSec = 120,
                    HowTo = "Đứng thẳng, siết bụng và mông, đẩy thanh đòn từ ngực trên lên quá đầu. Không ngả lưng ra sau." },
                new Exercise { ExerciseId = 10, Name = "Nâng tạ đơn sang ngang (Lateral Raise)", MuscleGroup = "Vai", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đứng thẳng, nâng hai tạ nhẹ sang ngang tới ngang vai, khuỷu hơi cong, hạ chậm." },
                new Exercise { ExerciseId = 11, Name = "Kéo cáp tay sau (Tricep Pushdown)", MuscleGroup = "Tay sau", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đứng trước giàn cáp, khuỷu tay ép sát người, đẩy tay cầm xuống tới khi duỗi thẳng tay rồi nâng lên chậm." },
                new Exercise { ExerciseId = 12, Name = "Duỗi tay sau qua đầu tạ đơn (Overhead Extension)", MuscleGroup = "Tay sau", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "shoulder", DefaultRestSec = 60,
                    HowTo = "Ngồi thẳng, hai tay giữ một tạ sau đầu, duỗi thẳng tay lên trên. Giữ khuỷu tay hướng về phía trước." },
                new Exercise { ExerciseId = 13, Name = "Chống đẩy ghế tay sau (Bench Dip)", MuscleGroup = "Tay sau", Category = ExerciseCategories.Push, Level = ExerciseLevels.Basic, EquipmentId = null, AvoidIfInjury = "shoulder", DefaultRestSec = 60,
                    HowTo = "Chống tay lên mép ghế sau lưng, hạ người xuống tới khi khuỷu tay vuông góc rồi đẩy lên." },
                new Exercise { ExerciseId = 14, Name = "Kéo cáp xuống trước ngực (Lat Pulldown)", MuscleGroup = "Lưng", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 90,
                    HowTo = "Ngồi thẳng, kéo thanh cáp xuống ngực trên, ép hai bả vai về sau. Không giật người ra sau." },
                new Exercise { ExerciseId = 15, Name = "Kéo cáp ngồi (Seated Cable Row)", MuscleGroup = "Lưng", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 90,
                    HowTo = "Ngồi lưng thẳng, kéo tay cầm về bụng, ép bả vai rồi duỗi tay ra chậm. Không cong lưng." },
                new Exercise { ExerciseId = 16, Name = "Chèo tạ đơn một tay (One-arm Dumbbell Row)", MuscleGroup = "Lưng", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = null, DefaultRestSec = 75,
                    HowTo = "Một tay và một gối chống ghế, lưng song song sàn, kéo tạ về hông, giữ 1 giây rồi hạ." },
                new Exercise { ExerciseId = 17, Name = "Chèo tạ đòn (Barbell Row)", MuscleGroup = "Lưng", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "back", DefaultRestSec = 120,
                    HowTo = "Gập người khoảng 45 độ, lưng thẳng, kéo thanh đòn về bụng dưới. Cần giữ lưng trung tính suốt hiệp." },
                new Exercise { ExerciseId = 18, Name = "Deadlift tạ đòn", MuscleGroup = "Lưng dưới", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "back", DefaultRestSec = 150,
                    HowTo = "Thanh đòn sát ống chân, lưng thẳng, đẩy hông về trước để đứng dậy. Bắt đầu với mức tạ nhẹ để học kỹ thuật." },
                new Exercise { ExerciseId = 19, Name = "Hít xà đơn (Pull-up) trên Power Rack", MuscleGroup = "Lưng", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "shoulder", DefaultRestSec = 120,
                    HowTo = "Nắm xà rộng hơn vai, kéo người lên tới khi cằm qua xà, hạ chậm tới khi duỗi tay." },
                new Exercise { ExerciseId = 20, Name = "Kéo cáp về mặt (Face Pull)", MuscleGroup = "Vai sau", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đặt cáp ngang mặt, kéo dây thừng về phía trán, mở hai khuỷu tay sang hai bên." },
                new Exercise { ExerciseId = 21, Name = "Bay vai sau trên máy (Rear Delt Fly)", MuscleGroup = "Vai sau", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 8, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Ngồi quay mặt vào máy, mở hai tay về sau ngang vai, giữ 1 giây rồi trở về chậm." },
                new Exercise { ExerciseId = 22, Name = "Cuốn tạ đơn tay trước (Dumbbell Curl)", MuscleGroup = "Tay trước", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đứng thẳng, khuỷu tay sát người, cuốn tạ lên tới vai rồi hạ chậm. Không đung đưa người." },
                new Exercise { ExerciseId = 23, Name = "Cuốn tạ búa (Hammer Curl)", MuscleGroup = "Tay trước", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Cầm tạ lòng bàn tay hướng vào nhau, cuốn lên tới vai, hạ chậm." },
                new Exercise { ExerciseId = 24, Name = "Cuốn cáp tay trước (Cable Curl)", MuscleGroup = "Tay trước", Category = ExerciseCategories.Pull, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đứng trước giàn cáp, cuốn thanh từ dưới lên ngực, khuỷu tay cố định." },
                new Exercise { ExerciseId = 25, Name = "Squat tạ đòn (Barbell Back Squat)", MuscleGroup = "Chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "knee,back", DefaultRestSec = 150,
                    HowTo = "Đặt thanh trên vai sau trong Power Rack, ngồi xuống tới khi đùi song song sàn, gối cùng hướng mũi chân." },
                new Exercise { ExerciseId = 26, Name = "Squat trên Smith Machine", MuscleGroup = "Chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 5, AvoidIfInjury = "knee", DefaultRestSec = 120,
                    HowTo = "Đứng dưới thanh Smith, chân hơi bước lên trước, ngồi xuống tới đùi song song sàn rồi đứng dậy." },
                new Exercise { ExerciseId = 27, Name = "Goblet Squat tạ đơn", MuscleGroup = "Chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "knee", DefaultRestSec = 90,
                    HowTo = "Ôm một tạ đơn trước ngực, ngồi xuống sâu, lưng thẳng, gót chân không nhấc khỏi sàn." },
                new Exercise { ExerciseId = 28, Name = "Đạp đùi 45 độ (Leg Press)", MuscleGroup = "Chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 9, AvoidIfInjury = "knee", DefaultRestSec = 90,
                    HowTo = "Ngồi tựa lưng, đặt chân rộng bằng vai, hạ bàn đạp tới khi gối vuông góc rồi đẩy lên, không khoá gối." },
                new Exercise { ExerciseId = 29, Name = "Romanian Deadlift tạ đơn", MuscleGroup = "Đùi sau", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "back", DefaultRestSec = 90,
                    HowTo = "Cầm hai tạ trước đùi, gối hơi chùng, đẩy hông ra sau cho tới khi căng đùi sau rồi đứng dậy." },
                new Exercise { ExerciseId = 30, Name = "Chùng chân bước tới (Walking Lunge)", MuscleGroup = "Chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 7, AvoidIfInjury = "knee", DefaultRestSec = 75,
                    HowTo = "Cầm hai tạ, bước dài lên trước, hạ gối sau gần sàn rồi bước tiếp chân kia." },
                new Exercise { ExerciseId = 31, Name = "Bulgarian Split Squat", MuscleGroup = "Chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Advanced, EquipmentId = 7, AvoidIfInjury = "knee", DefaultRestSec = 90,
                    HowTo = "Gác mu bàn chân sau lên ghế, hạ người bằng chân trước tới khi đùi song song sàn. Làm đủ số lần mỗi bên." },
                new Exercise { ExerciseId = 32, Name = "Đẩy hông với tạ đòn (Hip Thrust)", MuscleGroup = "Mông", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 6, AvoidIfInjury = null, DefaultRestSec = 90,
                    HowTo = "Tựa lưng trên vào ghế, thanh đòn trên hông, đẩy hông lên tới khi thân thẳng, siết mông 1 giây." },
                new Exercise { ExerciseId = 33, Name = "Đá chân sau với cáp (Cable Kickback)", MuscleGroup = "Mông", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đeo dây cáp vào cổ chân, đá chân thẳng ra sau, siết mông rồi đưa về chậm." },
                new Exercise { ExerciseId = 34, Name = "Nhón bắp chân trên Smith (Calf Raise)", MuscleGroup = "Bắp chân", Category = ExerciseCategories.Legs, Level = ExerciseLevels.Basic, EquipmentId = 5, AvoidIfInjury = null, DefaultRestSec = 60,
                    HowTo = "Đứng dưới thanh Smith, nhón gót lên cao nhất, giữ 1 giây rồi hạ xuống chậm." },
                new Exercise { ExerciseId = 35, Name = "Plank", MuscleGroup = "Bụng", Category = ExerciseCategories.Core, Level = ExerciseLevels.Basic, EquipmentId = null, AvoidIfInjury = null, DefaultRestSec = 45,
                    HowTo = "Chống khuỷu tay và mũi chân, thân thẳng từ đầu tới gót, siết bụng. Số lần ghi theo giây." },
                new Exercise { ExerciseId = 36, Name = "Gập bụng với cáp (Cable Crunch)", MuscleGroup = "Bụng", Category = ExerciseCategories.Core, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = "back", DefaultRestSec = 60,
                    HowTo = "Quỳ trước giàn cáp, giữ dây thừng cạnh đầu, gập bụng kéo khuỷu tay về phía gối." },
                new Exercise { ExerciseId = 37, Name = "Nâng chân treo xà (Hanging Leg Raise)", MuscleGroup = "Bụng", Category = ExerciseCategories.Core, Level = ExerciseLevels.Advanced, EquipmentId = 6, AvoidIfInjury = "shoulder", DefaultRestSec = 60,
                    HowTo = "Treo người trên xà, nâng hai chân lên ngang hông, hạ chậm và không đung đưa." },
                new Exercise { ExerciseId = 38, Name = "Chống xoay với cáp (Pallof Press)", MuscleGroup = "Bụng", Category = ExerciseCategories.Core, Level = ExerciseLevels.Basic, EquipmentId = 10, AvoidIfInjury = null, DefaultRestSec = 45,
                    HowTo = "Đứng nghiêng với giàn cáp, đẩy tay cầm thẳng ra trước ngực và giữ người không bị xoay." },
                new Exercise { ExerciseId = 39, Name = "Dead Bug", MuscleGroup = "Bụng", Category = ExerciseCategories.Core, Level = ExerciseLevels.Basic, EquipmentId = null, AvoidIfInjury = null, DefaultRestSec = 45,
                    HowTo = "Nằm ngửa, tay chân giơ lên, duỗi tay và chân đối diện ra xa, lưng dưới luôn áp sàn." },
                new Exercise { ExerciseId = 40, Name = "Đi bộ dốc / chạy bộ trên máy Technogym", MuscleGroup = "Cardio", Category = ExerciseCategories.Cardio, Level = ExerciseLevels.Basic, EquipmentId = 1, AvoidIfInjury = "knee", DefaultRestSec = 0,
                    HowTo = "Khởi động 3 phút đi bộ, sau đó đi dốc 8-12% hoặc chạy nhẹ ở mức vẫn nói chuyện được." },
                new Exercise { ExerciseId = 41, Name = "Chạy bộ trên máy Life Fitness", MuscleGroup = "Cardio", Category = ExerciseCategories.Cardio, Level = ExerciseLevels.Basic, EquipmentId = 2, AvoidIfInjury = "knee", DefaultRestSec = 0,
                    HowTo = "Khởi động 3 phút đi bộ, sau đó chạy nhẹ hoặc xen kẽ 1 phút nhanh, 2 phút chậm." },
                new Exercise { ExerciseId = 42, Name = "Đạp xe Matrix CXP", MuscleGroup = "Cardio", Category = ExerciseCategories.Cardio, Level = ExerciseLevels.Basic, EquipmentId = 3, AvoidIfInjury = null, DefaultRestSec = 0,
                    HowTo = "Chỉnh yên ngang hông, đạp đều ở mức vừa, có thể xen kẽ 30 giây đạp nhanh." },
                new Exercise { ExerciseId = 43, Name = "Chèo thuyền Concept2", MuscleGroup = "Cardio", Category = ExerciseCategories.Cardio, Level = ExerciseLevels.Basic, EquipmentId = 4, AvoidIfInjury = "back", DefaultRestSec = 0,
                    HowTo = "Đẩy chân trước, ngả người nhẹ rồi mới kéo tay. Giữ lưng thẳng suốt động tác." }
            };
        }
    }
}
