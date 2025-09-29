namespace SIMAPI.Data.Entities
{

    public partial class ProductCommission
    {
        public int ProductCommissionId { get; set; }
        public int ProductId { get; set; }

        public decimal CommissionToAgent { get; set; }
        public decimal CommissionToManager { get; set; }

        public short? IsActive { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
