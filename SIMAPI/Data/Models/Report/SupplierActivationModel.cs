namespace SIMAPI.Data.Models.Report
{
    public class SupplierActivationModel : BaseNetworkCodeModel
    {
        public string? SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? AccountName { get; set; }
    }

    public class LowStockModel 
    {
        public string? ProductName { get; set; }
        public int? Quantity { get; set; }
    }
}
