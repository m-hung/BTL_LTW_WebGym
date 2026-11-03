namespace WebGym.Models
{
    // Dữ liệu demo. Sau này có thể chuyển sang bảng trong database.
    public static class ProgramData
    {
        public static ProgramInfo? Find(string? slug) =>
            All.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

        public static readonly List<ProgramInfo> All = new()
        {
            new ProgramInfo {
                Slug="gym", Name="Gym", Title2="xây sức mạnh", Tag="Sức mạnh • Hình thể",
                Tagline="Tập tạ có giáo án, có người kèm và có tiến bộ đo được sau từng tuần.",
                Image="gym.jpg", Duration="60 phút", Level="Mọi trình độ", Intensity="Trung bình – Cao", Capacity="Tối đa 12 người",
                Coach="Alex Tokuda", CoachRole="Strength & Conditioning",
                Intro="Chương trình tập tạ theo giáo án từng giai đoạn, tập trung vào các bài compound nền tảng như squat, deadlift, bench press. Huấn luyện viên sửa tư thế trực tiếp để bạn tăng sức mạnh an toàn.",
                Audience=new(){"Người mới muốn học tập tạ đúng kỹ thuật","Người muốn tăng cơ, giảm mỡ có lộ trình","Người tập lâu năm cần phá vỡ giai đoạn chững lại"},
                Benefits=new(){
                    new("Tăng cơ rõ rệt","Giáo án tăng tạ dần theo tuần giúp cơ bắp phát triển đều."),
                    new("Đúng kỹ thuật","Huấn luyện viên chỉnh tư thế từng bài, hạn chế chấn thương."),
                    new("Theo dõi tiến bộ","Ghi lại mức tạ và số lần tập để bạn thấy mình mạnh lên."),
                    new("Đốt mỡ bền vững","Cơ bắp nhiều hơn giúp cơ thể tiêu hao năng lượng cả khi nghỉ.")},
                Steps=new(){
                    new("Khởi động","Làm nóng khớp và kích hoạt nhóm cơ chính.","10 phút"),
                    new("Bài chính","Squat, deadlift hoặc bench press theo ngày tập.","30 phút"),
                    new("Bài bổ trợ","Các bài phụ cho nhóm cơ yếu và cơ lõi.","15 phút"),
                    new("Giãn cơ","Thả lỏng và hướng dẫn phục hồi.","5 phút")},
                Slots=new(){ new("Thứ 2","19:00","Alex Tokuda",6), new("Thứ 4","06:00","Alex Tokuda",4), new("Thứ 6","18:30","Alex Tokuda",8) },
                Bring=new(){"Giày thể thao đế bằng","Khăn và bình nước","Găng tay tập tạ (nếu có)"},
                Faqs=new(){
                    new("Tôi chưa từng tập tạ có theo được không?","Được. Lớp có bài hướng dẫn cho người mới và mức tạ điều chỉnh theo sức bạn."),
                    new("Mỗi tuần nên tập mấy buổi?","Người mới nên tập 3 buổi mỗi tuần để cơ thể kịp phục hồi."),
                    new("Có cần tự mang tạ không?","Không. Phòng tập có đủ thiết bị.")}
            },
            new ProgramInfo {
                Slug="hiit", Name="HIIT", Title2="đốt mỡ bùng nổ", Tag="Đốt mỡ • Sức bền",
                Tagline="Mỗi buổi 45 phút, xen kẽ gắng sức và nghỉ ngắn để đốt năng lượng tối đa.",
                Image="hiit.jpg", Duration="45 phút", Level="Trung bình", Intensity="Cao", Capacity="Tối đa 16 người",
                Coach="Mina Tran", CoachRole="HIIT & Functional",
                Intro="Các vòng tập cường độ cao xen kẽ thời gian nghỉ ngắn, kết hợp động tác toàn thân. Buổi tập ngắn gọn nhưng đòi hỏi bạn tập trung hết sức.",
                Audience=new(){"Người bận rộn cần buổi tập ngắn, hiệu quả","Người muốn giảm mỡ và tăng sức bền tim phổi","Người thích tập theo nhóm, nhiều năng lượng"},
                Benefits=new(){
                    new("Đốt mỡ nhanh","Nhịp tim cao giúp tiêu hao nhiều năng lượng trong thời gian ngắn."),
                    new("Tăng sức bền","Tim phổi khỏe hơn, ít mệt khi vận động hằng ngày."),
                    new("Không nhàm chán","Mỗi buổi một bộ bài khác nhau, luôn có thử thách mới."),
                    new("Điều chỉnh theo sức","Mỗi bài có phiên bản nhẹ hơn cho người mới.")},
                Steps=new(){
                    new("Khởi động","Tăng dần nhịp tim và làm nóng khớp.","7 phút"),
                    new("Vòng tập 1","Gắng sức 40 giây, nghỉ 20 giây.","15 phút"),
                    new("Vòng tập 2","Đổi bài, tăng độ khó và tốc độ.","15 phút"),
                    new("Hạ nhịp tim","Đi bộ nhẹ và giãn cơ.","8 phút")},
                Slots=new(){ new("Thứ 2","06:00","Mina Tran",4), new("Thứ 5","06:00","Mina Tran",3), new("Thứ 7","08:00","Mina Tran",12) },
                Bring=new(){"Giày thể thao ôm chân","Quần áo thoáng, thấm mồ hôi","Khăn và bình nước lớn"},
                Faqs=new(){
                    new("HIIT có quá nặng với người mới không?","Bạn có thể chọn phiên bản nhẹ của từng bài và nghỉ khi cần."),
                    new("Tôi có nên ăn trước khi tập?","Nên ăn nhẹ trước buổi tập 1 đến 2 tiếng."),
                    new("Một tuần tập mấy buổi là đủ?","2 đến 3 buổi, cách nhau ít nhất một ngày.")}
            },
            new ProgramInfo {
                Slug="boxing", Name="Boxing", Title2="phản xạ & bùng nổ", Tag="Phản xạ • Bùng nổ",
                Tagline="Học đòn đấm cơ bản, di chuyển chân và xả stress sau một ngày dài.",
                Image="boxing.jpg", Duration="60 phút", Level="Mọi trình độ", Intensity="Cao", Capacity="Tối đa 14 người",
                Coach="Lil Baby", CoachRole="Boxing Coach",
                Intro="Lớp boxing fitness không đối kháng: học tư thế, các đòn cơ bản, phối hợp tay chân rồi tập trên bao cát. Bạn vừa tập kỹ thuật vừa đốt năng lượng.",
                Audience=new(){"Người muốn học võ cơ bản để tự tin hơn","Người cần cách xả stress hiệu quả","Người muốn cải thiện phản xạ và phối hợp"},
                Benefits=new(){
                    new("Học kỹ thuật thật","Jab, cross, hook, uppercut và di chuyển chân đúng chuẩn."),
                    new("Xả stress","Đấm bao cát giúp giải tỏa căng thẳng sau giờ làm."),
                    new("Săn chắc toàn thân","Vai, lưng, cơ lõi và chân đều hoạt động liên tục."),
                    new("Không đối kháng","Chỉ tập với bao và đích đấm, an toàn cho người mới.")},
                Steps=new(){
                    new("Khởi động","Nhảy dây và xoay khớp vai, hông.","10 phút"),
                    new("Kỹ thuật","Học và luyện các đòn cơ bản theo cặp đích đấm.","20 phút"),
                    new("Bao cát","Các hiệp 3 phút với bao cát.","20 phút"),
                    new("Giãn cơ","Thả lỏng vai, tay và lưng.","10 phút")},
                Slots=new(){ new("Thứ 2","17:30","Lil Baby",8), new("Thứ 5","18:00","Lil Baby",8), new("Thứ 7","16:00","Lil Baby",10) },
                Bring=new(){"Giày thể thao đế mềm","Băng quấn tay (phòng có bán)","Khăn và bình nước"},
                Faqs=new(){
                    new("Tôi có phải đấm với người khác không?","Không. Lớp chỉ tập với bao cát và đích đấm."),
                    new("Có cần mang găng tay không?","Phòng tập có găng cho mượn. Bạn nên mang băng quấn tay riêng."),
                    new("Lớp có phù hợp với nữ không?","Phù hợp. Cường độ điều chỉnh được theo sức mỗi người.")}
            },
            new ProgramInfo {
                Slug="yoga", Name="Yoga", Title2="cân bằng & linh hoạt", Tag="Cân bằng • Linh hoạt",
                Tagline="Thở sâu, giãn cơ và lấy lại cân bằng cho cơ thể lẫn tâm trí.",
                Image="yoga.jpg", Duration="60 phút", Level="Mọi trình độ", Intensity="Nhẹ – Trung bình", Capacity="Tối đa 20 người",
                Coach="Thu Hà", CoachRole="Huấn luyện viên Yoga",
                Intro="Chuỗi động tác yoga kết hợp hơi thở giúp tăng độ dẻo, cải thiện tư thế và giảm căng thẳng. Lớp phù hợp để phục hồi sau những buổi tập nặng.",
                Audience=new(){"Người ngồi nhiều, hay đau mỏi cổ vai gáy","Người tập nặng cần ngày phục hồi","Người muốn thư giãn và ngủ ngon hơn"},
                Benefits=new(){
                    new("Dẻo dai hơn","Các tư thế giãn sâu giúp cải thiện biên độ vận động."),
                    new("Tư thế tốt hơn","Cột sống và cơ lõi mạnh hơn, giảm đau lưng."),
                    new("Giảm căng thẳng","Hơi thở chậm giúp đầu óc nhẹ nhõm."),
                    new("Phục hồi cơ","Hỗ trợ cơ bắp hồi phục sau các buổi tập nặng.")},
                Steps=new(){
                    new("Thở và định tâm","Làm quen hơi thở, thả lỏng cơ thể.","10 phút"),
                    new("Chuỗi vận động","Các tư thế nối tiếp nhịp nhàng, làm nóng cơ.","25 phút"),
                    new("Tư thế giữ","Giữ lâu các tư thế để giãn sâu.","15 phút"),
                    new("Thư giãn cuối buổi","Nằm thả lỏng toàn thân.","10 phút")},
                Slots=new(){ new("Thứ 3","06:30","Thu Hà",10), new("Thứ 5","19:30","Thu Hà",10), new("Thứ 7","09:30","Thu Hà",15) },
                Bring=new(){"Quần áo co giãn thoải mái","Thảm yoga (phòng có sẵn)","Bình nước"},
                Faqs=new(){
                    new("Tôi không dẻo có tập được không?","Được. Yoga giúp bạn dẻo dần, không cần dẻo sẵn."),
                    new("Nên ăn gì trước khi tập?","Tránh ăn no, nên ăn nhẹ trước buổi tập ít nhất 2 tiếng."),
                    new("Tập yoga có giảm cân không?","Yoga hỗ trợ tư thế và giảm stress. Để giảm mỡ, bạn nên kết hợp thêm HIIT hoặc Gym.")}
            }
        };
    }
}
