namespace WebGym.Models.DTOs.Member
{
    public class ClassScheduleItemDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public string? TrainerAvatar { get; set; }
        public string? RoomName { get; set; }
        public string? RoomLocation { get; set; }
        public string Schedule { get; set; } = string.Empty; // Ví dụ: "Thứ 2 - 4 - 6 (06:30 - 07:45)"
        public int MaxCapacity { get; set; } = 20;
        public int CurrentCapacity { get; set; } = 0;
        public int AvailableSlots => Math.Max(0, MaxCapacity - CurrentCapacity);
        public string Status { get; set; } = "Open"; // "Open" hoặc "Full"
        public bool IsOpen => Status == "Open" && AvailableSlots > 0;
        public string? ImageUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        // Trạng thái đối với hội viên hiện tại đang đăng nhập
        public bool IsBookedByCurrentUser { get; set; } = false;
        public int? CurrentUserBookingId { get; set; }
    }

    public class MyClassBookingItemDto
    {
        public int ClassBookingId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public string? TrainerAvatar { get; set; }
        public string? RoomName { get; set; }
        public string? RoomLocation { get; set; }
        public string Schedule { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = "Confirmed"; // 'Confirmed', 'Attended', 'Cancelled'
        public bool CanCancel => Status == "Confirmed";
    }

    public class ClassBookingPageViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string ActiveTab { get; set; } = "schedule"; // "schedule" hoặc "my-classes"
        public List<ClassScheduleItemDto> AvailableClasses { get; set; } = new();
        public List<MyClassBookingItemDto> MyBookedClasses { get; set; } = new();
    }
}
