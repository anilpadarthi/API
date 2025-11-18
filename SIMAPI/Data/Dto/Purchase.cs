namespace SIMAPI.Data.Dto
{
    public class PurchaseInvoiceCreateDto
    {
        public int? PurchaseInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int SupplierId { get; set; }
        public decimal TotalAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<PurchaseInvoiceItemCreateDto> Items { get; set; }
    }

    public class PurchaseInvoiceItemCreateDto
    {
        public int? PurchaseInvoiceItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
    }

}
