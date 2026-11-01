namespace WebGym.Models.DTOs.Admin
{
    public class DashboardStatsDto
    {
        public int TotalMembers { get; set; }
        public int TotalTrainers { get; set; }
        public int TotalActiveBookings { get; set; }
        public decimal TotalRevenueThisMonth { get; set; }
        public decimal TotalRevenueOverall { get; set; }

        public List<MonthlyRevenueItem> MonthlyRevenueChart { get; set; } = new();
        public List<RecentBookingItem> RecentBookings { get; set; } = new();
    }

    public class MonthlyRevenueItem
    {
        public string MonthName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class RecentBookingItem
    {
        public int BookingId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class RevenueReportDto
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;
        public int? Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalTransactions { get; set; }
    }
}
