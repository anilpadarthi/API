namespace SIMAPI.Data.Dto
{
    public class NetworkDto
    {
        public int NetworkId { get; set; }
        public int BaseNetworkId { get; set; }
        public int SupplierId { get; set; }

        public string NetworkName { get; set; } 

        public string NetworkCode { get; set; } 
        public string SkuCode { get; set; } 

        public short Status { get; set; }
    }
}
