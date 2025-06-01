namespace SIMAPI.Data.Dto
{
    public class OrderPaymentDto
    {
        public int OrderId { get; set; }
        public int ShopId { get; set; }
        public int? UserId { get; set; }
        public int? OrderPaymentId { get; set; }
        public string? Status { get; set; }
        public string? Comments { get; set; }
        public string PaymentMode { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public IFormFile? ReferenceImage { get; set; }
    }
}
