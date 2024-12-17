namespace SIMAPI.Data.Models.CommissionStatement
{
    public class CommissionShopListModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string AreaCode { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string PayableName { get; set; }
        public string VatNumber { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime TopupDate { get; set; }
        public DateTime DisplayDate { get; set; }
    }
}
