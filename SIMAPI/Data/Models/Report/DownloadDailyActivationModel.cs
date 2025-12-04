using System.ComponentModel.DataAnnotations.Schema;

namespace SIMAPI.Data.Models.Report.InstantReport
{
    public class DownloadDailyActivationModel
    {
        public string? Supplier { get; set; }
        public int? AreaId { get; set; }
        public string? AreaName { get; set; }
        public int? ShopId { get; set; }
        public string? ShopName { get; set; }
        [NotMapped]
        public int? AgentId { get; set; }
        [NotMapped]
        public string? AgentName { get; set; }
        [NotMapped]
        public string? ManagerName { get; set; }
        public int? SimId { get; set; }
        public string? IMEI { get; set; }
        public string? PCNNO { get; set; }
        public string? Network { get; set; }
        public DateTime? ActivatedDate { get; set; }
    }
}
