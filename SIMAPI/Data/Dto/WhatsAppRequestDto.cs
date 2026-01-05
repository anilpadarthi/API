namespace SIMAPI.Data.Dto
{
    public class WhatsAppRequestDto
    {
        public int? UserId { get; set; }
        public string? UserType { get; set; }
        public string? RequestType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
