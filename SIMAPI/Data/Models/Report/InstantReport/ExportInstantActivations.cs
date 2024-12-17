using CsvHelper.Configuration;
namespace SIMAPI.Data.Models.Report.InstantReport
{
    public sealed class ExportInstantActivations : ClassMap<ShopInstantActivationReportModel>
    {
        public ExportInstantActivations()
        {
            Map(m => m.AreaId).Name("Area ID");
            Map(m => m.AreaName).Name("Area Name");
            Map(m => m.ShopId).Name("Shop ID");
            Map(m => m.ShopName).Name("Shop Name");
            Map(m => m.AgentId).Name("Agent ID");
            Map(m => m.AgentName).Name("Agent Name");
            Map(m => m.ManagerName).Name("Manager Name");
            Map(m => m.Network).Name("Network");
            Map(m => m.IMEI).Name("IMEI");
            Map(m => m.PCNNO).Name("PCNNO");
            Map(m => m.ActivatedDate).Name("Activated Date");

        }
    }
}
