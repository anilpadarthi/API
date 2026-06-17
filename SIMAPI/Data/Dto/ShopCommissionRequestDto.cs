namespace SIMAPI.Data.Dto
{
    public partial class ShopCommissionRequestDto
    {

        public int? shopCommissionRequestId { get; set; }
        public int? commissionChangeRequestId { get; set; }
        public int shopId { get; set; }
        public string? remarks { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public short? status { get; set; }
        public int? loggedInUserId { get; set; }
        public int? userRoleId { get; set; }
        public short? isMobileShop { get; set; }
    }
}
