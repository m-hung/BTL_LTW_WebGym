namespace WebGym.Models
{
    // Dữ liệu demo. Sau này có thể chuyển sang bảng Trainers trong database.
    public static class TrainerData
    {
        public static TrainerInfo? Find(string? slug) =>
            All.FirstOrDefault(t => t.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

        public static readonly List<TrainerInfo> All = new()
        {
            new TrainerInfo {
                Slug="alex-tokuda", FirstName="Alex", LastName="Tokuda", Role="Strength & Conditioning",
                Badge="10+ năm kinh nghiệm", Image="coach1.jpg", Years="10+", Clients="300+", ClassesPerWeek="6",
                Bio="Alex theo đuổi huấn luyện sức mạnh hơn mười năm. Anh xây giáo án từng giai đoạn, sửa kỹ thuật từng bài tập và theo sát tiến bộ của học viên để tăng cơ, giảm mỡ một cách an toàn.",
                Quote="Sức mạnh không đến từ việc nâng thật nặng ngay hôm nay, mà từ việc nâng đúng mỗi ngày.",
                Specialties=new(){"Tập tạ nền tảng","Tăng cơ & giảm mỡ","Sửa tư thế","Lên giáo án cá nhân","Tập lại sau thời gian nghỉ"},
                Achievements=new(){
                    new("Hơn 10 năm huấn luyện","Làm việc với người mới lẫn vận động viên nghiệp dư."),
                    new("Chuyên gia sức mạnh","Phụ trách chương trình Gym tại Pulse Gym."),
                    new("Hơn 300 học viên","Đã đồng hành xây dựng thể hình và thói quen tập luyện.")},
                ProgramSlugs=new(){"gym"},
                Slots=new(){ new("Thứ 2","19:00","Gym",6), new("Thứ 4","06:00","Gym",4), new("Thứ 6","18:30","Gym",8) }
            },
            new TrainerInfo {
                Slug="mina-tran", FirstName="Mina", LastName="Tran", Role="HIIT & Functional",
                Badge="NASM Certified", Image="coach2.jpg", Years="6+", Clients="200+", ClassesPerWeek="5",
                Bio="Mina huấn luyện HIIT và vận động chức năng, tập trung vào những bài toàn thân giúp đốt mỡ, tăng sức bền và cải thiện cách bạn vận động hằng ngày. Cô đạt chứng chỉ NASM.",
                Quote="Mười lăm phút khó nhất sẽ cho bạn cả ngày nhẹ nhõm.",
                Specialties=new(){"HIIT cường độ cao","Vận động chức năng","Giảm mỡ toàn thân","Tăng sức bền tim phổi","Bài tập tại chỗ"},
                Achievements=new(){
                    new("Chứng chỉ NASM","Được cấp chứng chỉ huấn luyện cá nhân NASM."),
                    new("Phụ trách lớp HIIT","Thiết kế bộ bài mới mỗi tuần để lớp luôn có thử thách."),
                    new("Hơn 200 học viên","Từ người mới bắt đầu đến người đã tập nhiều năm.")},
                ProgramSlugs=new(){"hiit"},
                Slots=new(){ new("Thứ 2","06:00","HIIT",4), new("Thứ 5","06:00","HIIT",3), new("Thứ 7","08:00","HIIT",12) }
            },
            new TrainerInfo {
                Slug="lil-baby", FirstName="Lil", LastName="Baby", Role="Boxing Coach",
                Badge="Vô địch toàn quốc", Image="coach3.jpg", Years="8+", Clients="250+", ClassesPerWeek="5",
                Bio="Lil Baby là nhà vô địch toàn quốc, chuyên dạy boxing từ cơ bản. Cô hướng dẫn tư thế, các đòn đấm, di chuyển chân và giúp học viên tự tin hơn sau mỗi buổi tập.",
                Quote="Mỗi cú đấm đẹp bắt đầu từ đôi chân vững và một cái đầu bình tĩnh.",
                Specialties=new(){"Boxing cơ bản","Phối hợp tay chân","Bao cát & đích đấm","Xả stress","Thể lực cho võ thuật"},
                Achievements=new(){
                    new("Vô địch toàn quốc","Thành tích thi đấu boxing cấp quốc gia."),
                    new("Phụ trách lớp Boxing","Giảng dạy bài bản, an toàn, không đối kháng."),
                    new("Hơn 250 học viên","Giúp nhiều người xây thể lực và sự tự tin.")},
                ProgramSlugs=new(){"boxing"},
                Slots=new(){ new("Thứ 2","17:30","Boxing",8), new("Thứ 5","18:00","Boxing",8), new("Thứ 7","16:00","Boxing",10) }
            }
        };
    }
}
