namespace SIMAPI.Data.Dto
{
    public class UserDocumentDto
    {
        public int UserDocumentId { get; set; }
        public int UserId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public IFormFile? DocumentImageFile { get; set; }
        public short Status { get; set; }        
        public int? CreatedBy { get; set; }        
        public int? UpdatedBy { get; set; }        
        
    }
}
