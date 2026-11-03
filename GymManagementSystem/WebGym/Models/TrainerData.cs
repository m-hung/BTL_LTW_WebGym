namespace WebGym.Models
{
    public static class TrainerData
    {
        public static TrainerInfo? Find(string? slug) =>
            All.FirstOrDefault(t => t.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

        public static readonly List<TrainerInfo> All = new();
    }
}
