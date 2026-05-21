using System.Text.Json.Serialization;

namespace SIMAPI.Data.Entities
{
    public  class SalaryCommissionConfiguration
    {
        public int? SalaryCommissionConfigurationId { get; set; }
        public int? UserId { get; set; }
        public bool? IsCommissionBasedOnCollectedAmount { get; set; } 
        public bool? IsCommissionBasedOnCutoffDate { get; set; } 
        public DateTime? PayslipDate { get; set; } 
    }
}
