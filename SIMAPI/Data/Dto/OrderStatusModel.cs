namespace SIMAPI.Data.Dto
{
    public partial class OrderStatusModel
    {
        public int OrderId { get; set; }
        public int? OrderStatusId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? ShippingModeId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public int? loggedInUserId { get; set; }
    }
}
