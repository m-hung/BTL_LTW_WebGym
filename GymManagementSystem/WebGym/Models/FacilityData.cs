namespace WebGym.Models
{
    // Dữ liệu demo (khu vực tập luyện / cơ sở vật chất).
    // Sau này thay bằng dữ liệu thật từ database của bạn.
    public static class FacilityData
    {
        public static FacilityInfo? Find(string? slug) =>
            All.FirstOrDefault(f => f.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

        static List<CrowdSlot> Crowd(params int[] l) => new()
        {
            new("05:00 – 08:00", l[0]), new("08:00 – 11:00", l[1]), new("11:00 – 14:00", l[2]),
            new("14:00 – 17:00", l[3]), new("17:00 – 20:30", l[4]), new("20:30 – 23:00", l[5])
        };

        public static readonly List<FacilityInfo> All = new()
        {
            new FacilityInfo {
                Slug="khu-ta-tu-do", Name="Tạ tự do", FullName="Khu tạ tự do", Title2="khu sức mạnh", Tag="Sức mạnh • Hình thể",
                Image="gym.jpg", Tagline="Giàn rack, tạ đơn, tạ đòn và sàn chịu lực cho những buổi tập nặng.",
                Intro="Khu vực rộng rãi dành cho tập tạ nền tảng như squat, deadlift, bench press. Thiết bị được bố trí theo từng nhóm cơ để bạn di chuyển thuận tiện và tập an toàn.",
                Area="350 m²", Capacity="60 người", Hours="05:00 – 23:00", Location="Tầng 1",
                Audience=new(){"Người tập tạ, tăng cơ và giảm mỡ","Người mới muốn làm quen với tạ đúng cách","Người cần không gian tập nặng, thoải mái"},
                Highlights=new(){
                    new("Sàn chịu lực","Lót cao su dày, giảm chấn khi nâng và thả tạ."),
                    new("Gương toàn khu","Dễ theo dõi và chỉnh tư thế khi tập."),
                    new("Bố trí theo nhóm cơ","Ngực, lưng, chân, vai tách khu rõ ràng."),
                    new("Huấn luyện viên hỗ trợ","Có người hướng dẫn kỹ thuật ngay tại khu vực.")},
                Equipments=new(){
                    new("Giàn rack squat","8 giàn • Kèm thanh safety"),
                    new("Ghế đẩy ngực","10 ghế • Chỉnh được độ dốc"),
                    new("Tạ đơn","2 – 50 kg • Đủ bộ"),
                    new("Tạ đòn & đĩa tạ","Tạ đòn Olympic • Đĩa 1,25 – 25 kg"),
                    new("Máy tập chuyên biệt","Kéo xà, đẩy đùi, ép ngực"),
                    new("Khu hỗ trợ","Phấn tập, đai lưng, dây kháng lực")},
                Rules=new(){"Lau thiết bị sau khi sử dụng","Cất tạ về đúng vị trí","Không thả tạ từ trên cao xuống sàn","Mang giày thể thao khi vào khu vực"},
                Crowd=Crowd(2,1,1,1,3,2)
            },
            new FacilityInfo {
                Slug="khu-cardio", Name="Cardio", FullName="Khu cardio", Title2="khu tim mạch", Tag="Đốt mỡ • Sức bền",
                Image="hiit.jpg", Tagline="Máy chạy, xe đạp và máy elliptical để tăng sức bền và đốt năng lượng.",
                Intro="Khu vực cardio thoáng, nhiều ánh sáng, có màn hình giải trí ở mỗi máy. Bạn có thể khởi động trước buổi tập hoặc dành riêng cho các buổi đốt mỡ.",
                Area="220 m²", Capacity="45 người", Hours="05:00 – 23:00", Location="Tầng 1",
                Audience=new(){"Người muốn giảm mỡ và tăng sức bền","Người cần khởi động trước khi tập tạ","Người tập nhẹ, phục hồi sau buổi tập nặng"},
                Highlights=new(){
                    new("Màn hình giải trí","Theo dõi nhịp tim, quãng đường và thời gian ngay trên máy."),
                    new("Điều hòa mát","Không gian thoáng, luôn dễ chịu khi tập lâu."),
                    new("Nhiều loại máy","Chọn máy phù hợp với khớp gối và sức khỏe của bạn."),
                    new("Gần khu tạ","Di chuyển giữa cardio và tạ tiện lợi.")},
                Equipments=new(){
                    new("Máy chạy bộ","16 máy • Có độ dốc"),
                    new("Xe đạp tập","10 xe • Đứng và ngồi"),
                    new("Máy elliptical","8 máy • Giảm áp lực khớp"),
                    new("Máy chèo thuyền","4 máy"),
                    new("Máy leo cầu thang","4 máy"),
                    new("Cảm biến nhịp tim","Dây đeo cho mượn tại quầy")},
                Rules=new(){"Lau máy sau khi dùng","Nên mang khăn và bình nước","Giới hạn 30 phút mỗi máy khi khu vực đông","Không dùng máy khi chưa cài đúng dây an toàn"},
                Crowd=Crowd(2,1,1,1,3,2)
            },
            new FacilityInfo {
                Slug="phong-boxing", Name="Boxing", FullName="Phòng boxing", Title2="phòng võ thuật", Tag="Phản xạ • Bùng nổ",
                Image="boxing.jpg", Tagline="Bao cát, đích đấm và sàn tập riêng cho các buổi boxing và xả stress.",
                Intro="Phòng tập riêng với dãy bao cát treo, khu đích đấm và khu tập bóng phản xạ. Phù hợp cho các lớp boxing nhóm lẫn khi bạn muốn tự luyện.",
                Area="140 m²", Capacity="24 người", Hours="05:00 – 23:00", Location="Tầng 2",
                Audience=new(){"Người học boxing cơ bản","Người muốn xả stress sau giờ làm","Người cần luyện phản xạ và phối hợp tay chân"},
                Highlights=new(){
                    new("Dãy bao cát","Nhiều loại bao với trọng lượng khác nhau."),
                    new("Sàn đệm êm","Giảm áp lực lên khớp khi di chuyển chân."),
                    new("Gương và đồng hồ hiệp","Dễ tập theo hiệp 3 phút."),
                    new("Không đối kháng","Chỉ tập với bao và đích, an toàn cho người mới.")},
                Equipments=new(){
                    new("Bao cát treo","10 bao • 30 – 50 kg"),
                    new("Đích đấm","Đích tay và đích bụng"),
                    new("Bóng phản xạ","Bóng đôi và bóng đơn"),
                    new("Găng tay","Cho mượn, nhiều cỡ"),
                    new("Băng quấn tay","Có bán tại quầy"),
                    new("Dây nhảy","Dùng khởi động")},
                Rules=new(){"Quấn tay trước khi đấm bao","Vệ sinh găng sau mỗi buổi","Giữ khoảng cách an toàn giữa các bao","Không tập đối kháng khi chưa có huấn luyện viên"},
                Crowd=Crowd(1,1,1,1,3,2)
            },
            new FacilityInfo {
                Slug="studio-yoga", Name="Yoga", FullName="Studio yoga", Title2="studio thư giãn", Tag="Cân bằng • Linh hoạt",
                Image="yoga.jpg", Tagline="Không gian yên tĩnh, ánh sáng dịu để giãn cơ, thở sâu và phục hồi.",
                Intro="Studio riêng biệt với sàn gỗ ấm, gương lớn và ánh sáng dịu. Bạn có thể tham gia lớp yoga hoặc tự giãn cơ, phục hồi sau những buổi tập nặng.",
                Area="120 m²", Capacity="25 người", Hours="05:00 – 23:00", Location="Tầng 2",
                Audience=new(){"Người ngồi nhiều, hay đau mỏi cổ vai gáy","Người tập nặng cần khu vực phục hồi","Người muốn thư giãn và cân bằng tinh thần"},
                Highlights=new(){
                    new("Sàn gỗ ấm","Thoải mái khi ngồi và nằm tập."),
                    new("Ánh sáng dịu","Thiết kế tạo cảm giác yên tĩnh."),
                    new("Cách âm tốt","Ít bị ảnh hưởng bởi tiếng ồn khu tập nặng."),
                    new("Dụng cụ đầy đủ","Không cần mang theo gì ngoài bản thân.")},
                Equipments=new(){
                    new("Thảm yoga","30 thảm"),
                    new("Gạch và dây yoga","Đủ cho cả lớp"),
                    new("Bóng thăng bằng","Nhiều kích cỡ"),
                    new("Con lăn giãn cơ","Foam roller các loại"),
                    new("Gối và chăn","Dùng cho thư giãn cuối buổi"),
                    new("Gương lớn","Một mặt tường")},
                Rules=new(){"Giữ yên lặng trong khu vực","Tắt chuông điện thoại","Tháo giày trước khi vào","Vệ sinh thảm sau khi dùng"},
                Crowd=Crowd(1,1,1,1,2,1)
            }
        };
    }
}
