namespace SIMAPI.Data.Entities
{
    public partial class BulkUploadFile
    {
        public int BulkUploadFileId { get; set; }
        public string? FilePath { get; set; }
        public string? FileStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? FileType { get; set; }
        public string? ExclusiveDate { get; set; }
        public string? FileName { get; set; }

    }
}
