namespace WebGym.Models
{
    public record Highlight(string Title, string Text);
    public record Equipment(string Name, string Detail);
    public record CrowdSlot(string Time, int Level);   // 1 = vắng, 2 = vừa, 3 = đông

    public class FacilityInfo
    {
        public string Slug { get; init; } = "";
        public string Name { get; init; } = "";        // tên ngắn trên thẻ, ví dụ "Tạ tự do"
        public string FullName { get; init; } = "";    // tên đầy đủ
        public string Title2 { get; init; } = "";      // dòng chữ viền ở hero
        public string Tag { get; init; } = "";
        public string Tagline { get; init; } = "";
        public string Image { get; init; } = "";
        public string Intro { get; init; } = "";
        public string Area { get; init; } = "";
        public string Capacity { get; init; } = "";
        public string Hours { get; init; } = "";
        public string Location { get; init; } = "";
        public List<string> Audience { get; init; } = new();
        public List<Highlight> Highlights { get; init; } = new();
        public List<Equipment> Equipments { get; init; } = new();
        public List<string> Rules { get; init; } = new();
        public List<CrowdSlot> Crowd { get; init; } = new();
    }
}
