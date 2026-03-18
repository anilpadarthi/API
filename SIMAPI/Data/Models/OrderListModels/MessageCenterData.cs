namespace SIMAPI.Data.Models.OrderListModels
{
    public class MessageCenterData
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaidDate { get; set; }
    }
}
