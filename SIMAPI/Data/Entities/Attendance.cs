namespace SIMAPI.Data.Entities
{
    public partial class Attendance
    {
        public int AttendanceId { get; set; }
        public int? UserId { get; set; }
        public string AttendanceType { get; set; } 
        public string Comments { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime? DateOfAttendance { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
