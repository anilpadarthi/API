namespace SIMAPI.Data.Entities
{
    public partial class ShopAgreement
    {
        public int ShopAgreementId { get; set; }
        public int ShopId { get; set; }

        public DateTime FromDate { get; set; } 

        public DateTime ToDate { get; set; }

        public string? AgreementNotes { get; set; }

        public int AgreementBy { get; set; }
        public int? ApprovedBy { get; set; }
        public short? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
}
