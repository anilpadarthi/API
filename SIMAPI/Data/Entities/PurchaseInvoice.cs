namespace SIMAPI.Data.Entities
{
    public partial class PurchaseInvoice
    {
        public int PurchaseInvoiceId { get; set; }
        public int SupplierId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CreatedBy { get; set; }

    }
}
