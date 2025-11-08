namespace SIMAPI.Data.Entities
{
    public partial class UserSalaryTransaction
    {
        public int? UserSalaryTransactionID { get; set; }
        public int UserId { get; set; }
        public string? Mode { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string? Comments { get; set; }
        public short? IsActive { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
