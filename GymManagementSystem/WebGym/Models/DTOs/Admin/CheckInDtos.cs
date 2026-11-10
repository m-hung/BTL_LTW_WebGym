namespace WebGym.Models.DTOs.Admin
{
    // ================= DTOs CHO ĐIỂM DANH (CHECK-IN) =================
    public class CheckInAdminViewModel
    {
        public int TodayCount { get; set; }
        public int ValidCount { get; set; }
        public int WarningCount { get; set; }
        public List<CheckInHistoryItemDto> TodayHistory { get; set; } = new();
    }

    public class CheckInHistoryItemDto
    {
        public int CheckInId { get; set; }
        public int MemberId { get; set; }
        public string CardCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty; // Giờ vào (HH:mm:ss)
        public string FullTime { get; set; } = string.Empty; // dd/MM/yyyy HH:mm
        public string Method { get; set; } = "Mã QR Code";
        public bool IsValid { get; set; } = true;
        public int DaysRemaining { get; set; }
    }

    public class CheckInProcessRequest
    {
        public string Code { get; set; } = string.Empty;
        public string? Method { get; set; } // "Mã QR Code" hoặc "Mã thẻ thủ công"
    }

    public class CheckInProcessResult
    {
        public bool Success { get; set; }
        public bool IsValid { get; set; }
        public bool IsDuplicate { get; set; } = false;
        public string? ErrorCode { get; set; } // "NOT_FOUND", "EXPIRED", "DUPLICATE", etc.
        public string Message { get; set; } = string.Empty;
        public int? CheckInId { get; set; }
        public int? MemberId { get; set; }
        public string CardCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public string PackageName { get; set; } = string.Empty;
        public int DaysRemaining { get; set; }
        public string ExpiryDate { get; set; } = string.Empty;
        public string CheckInTime { get; set; } = string.Empty;
        public string CheckInHour { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
    }
}
