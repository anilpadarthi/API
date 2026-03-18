namespace SIMAPI.Data.Dto
{
    public partial class ShopCommissionRequestDto
    {

        public int? ShopCommissionRequestId { get; set; }
        public int ShopId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string? Status { get; set; }
        public int? loggedInUserId { get; set; }
        public short isMobileShop { get; set; }
    }
}
