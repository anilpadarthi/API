namespace SIMAPI.Data.Entities
{

    public partial class CategoryCommission
    {
        public int CategoryCommissionId { get; set; }
        public int CategoryId { get; set; }

        public decimal CommissionPercent { get; set; }

        public short? IsActive { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }


    }
}
