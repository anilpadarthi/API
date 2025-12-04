namespace SIMAPI.Data.Models.Topup
{
    public class TopupResponse
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public int SimId { get; set; }
        public string IMEI { get; set; }
        public string Network { get; set; }
        public decimal Commission { get; set; }

    }

    public class TopupSaveResponse
    {
        public bool Status  { get; set; }
        public string Description { get; set; }

    }
}
