namespace SIMAPI.Data.Entities
{
    public partial class ErrorInfo
    {
        public int ErrorInfoId { get; set; }
        public string ErrorMessage { get; set; } 
        public string StackTrace { get; set; } 
        public string Method { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
