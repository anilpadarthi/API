namespace SIMAPI.Data.Entities
{
    public partial class WhatsAppRequest
    {
        public int WhatsAppRequestId { get; set; }
        public int? UserId { get; set; }
        public string? UserType { get; set; } 
        public string RequestType { get; set; }
        public string Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
