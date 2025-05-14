namespace SIMAPI.Data.Models.Topup
{
    public class TopupResponse
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
        public int SimId { get; set; }
        public string ICICD { get; set; }
        public string Network { get; set; }
        public string Commission { get; set; }

    }
}
