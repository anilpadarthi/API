namespace SIMAPI.Data.Dto
{
    public class GetReportRequest
    {
        public string? filterMode { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public int? userId { get; set; }
        public int? managerId { get; set; }
        public int? loggedInUserId { get; set; }
        public string? loggedInUserRole { get; set; }
        public int? userRoleId { get; set; }
        public string? userRole { get; set; }
        public int? areaId { get; set; }
        public int? shopId { get; set; }
        public string? reportType { get; set; }
        public string? activationType { get; set; }
        public string? filterType { get; set; }
        public int? filterId { get; set; }
        public int? filterUserRoleId { get; set; }
        public bool isInstantActivation { get; set; }
        public bool isSpamActivation { get; set; }
        public bool isOptedForCheques { get; set; }
    }
}
