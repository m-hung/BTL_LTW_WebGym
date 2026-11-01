using WebGym.Models.Entities;

namespace WebGym.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database schema and model builder seeds are applied
            context.Database.EnsureCreated();
        }
    }
}
