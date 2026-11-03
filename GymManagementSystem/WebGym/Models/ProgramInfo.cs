namespace WebGym.Models
{
    public record Benefit(string Title, string Text);
    public record Step(string Title, string Text, string Duration);
    public record ClassSlot(string Day, string Time, string Coach, int Spots);
    public record Faq(string Question, string Answer);

    public class ProgramInfo
    {
        public string Slug { get; init; } = "";
        public string Name { get; init; } = "";
        public string Title2 { get; init; } = "";      // dòng chữ viền
        public string Tag { get; init; } = "";
        public string Tagline { get; init; } = "";
        public string Image { get; init; } = "";
        public string Intro { get; init; } = "";
        public string Duration { get; init; } = "";
        public string Level { get; init; } = "";
        public string Intensity { get; init; } = "";
        public string Capacity { get; init; } = "";
        public string Coach { get; init; } = "";
        public string CoachRole { get; init; } = "";
        public List<string> Audience { get; init; } = new();
        public List<Benefit> Benefits { get; init; } = new();
        public List<Step> Steps { get; init; } = new();
        public List<ClassSlot> Slots { get; init; } = new();
        public List<string> Bring { get; init; } = new();
        public List<Faq> Faqs { get; init; } = new();
    }
}
