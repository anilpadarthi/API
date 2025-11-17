namespace SIMAPI.Data.Entities
{
    public partial class PurchaseInvoiceItem
    {
        public int PurchaseInvoiceItemId { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}
