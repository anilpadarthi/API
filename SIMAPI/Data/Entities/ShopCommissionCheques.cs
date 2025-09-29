namespace SIMAPI.Data.Entities
{

    public partial class ShopCommissionCheques
    {
        public int Sno { get; set; }
        public int ShopId { get; set; }

        public string ChequeNumber { get; set; }

        public string? TotalAmount { get; set; }


        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? CommissionDate { get; set; }
        public bool? IsDelete { get; set; }


    }
}
