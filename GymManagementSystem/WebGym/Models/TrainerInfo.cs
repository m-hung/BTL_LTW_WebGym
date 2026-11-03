namespace WebGym.Models
{
    public record Achievement(string Title, string Detail);
    public record TrainerSlot(string Day, string Time, string ClassName, int Spots);

    public class TrainerInfo
    {
        public string Slug { get; init; } = "";
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public string Role { get; init; } = "";
        public string Badge { get; init; } = "";
        public string Image { get; init; } = "";
        public string Bio { get; init; } = "";
        public string Quote { get; init; } = "";
        public string Years { get; init; } = "";
        public string Clients { get; init; } = "";
        public string ClassesPerWeek { get; init; } = "";
        public List<string> Specialties { get; init; } = new();
        public List<Achievement> Achievements { get; init; } = new();
        public List<string> ProgramSlugs { get; init; } = new();
        public List<TrainerSlot> Slots { get; init; } = new();
    }
}
