namespace SIMAPI.Data.Models.Tracking
{
    public class TrackReportModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public int AreasVisited { get; set; }
        public int ShopsVisited { get; set; }
        public int ShopsSimsGiven { get; set; }
        public string TotalTime { get; set; }
        public string Login { get; set; }
        public string FirstShop { get; set; }
        public string LogOut { get; set; }
        public string AttendenceType { get; set; }
        public string Comments { get; set; }
       
    }
}
