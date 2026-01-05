namespace SIMAPI.Data.Models.Tracking
{
    public class AttendanceDto
    {
        public int UserId { get; set; }
        public DateTime DateOfAttendance { get; set; }
        public string? AttendanceType { get; set; }
        public string? Comments { get; set; }
    }
}
