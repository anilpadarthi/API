namespace SIMAPI.Data.Models.Retailer
{
    public class ActivationModel
    {
        public string BaseNetwork { get; set; }
        public int ActivationCount { get; set; }
    }

    public class ActivationDetaiListModel
    {
        public DateTime ActivatedDate { get; set; }
        public string IMEI { get; set; }
        public string PCNNO { get; set; }
        public string Network { get; set; }
    }

    public class SimGivenDetailListModel
    {
        public DateTime GivenDate { get; set; }
        public string IMEI { get; set; }
        public string PCNNO { get; set; }
        public string Network { get; set; }
    }

    public class RetailerCommissionListModel
    {
        public DateTime CommissionDate { get; set; }
        public decimal CommissionAmount { get; set; }
        public int ShopCommissionHistoryId { get; set; }
    }
}
