namespace SIMAPI.Data.Entities
{
    public class UserDocument
    {
        public int UserDocumentId { get; set; }
        public int UserId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string DocumentImage { get; set; }
        public short Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
    }
}
