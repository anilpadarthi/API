namespace SIMAPI.Data.Models.OrderListModels
{
    public class PaymentReceiptModel
    {
        public string ReceiptNo { get; set; }
        public string CustomerName { get; set; }
        //public string CustomerPhone { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }
        public string ShopEmail { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public int OrderId { get; set; }
    }
}
