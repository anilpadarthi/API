namespace SIMAPI.Data.Dto
{
    public class BulkUploadDto
    {
        public IFormFile? ImportFile { get; set; }
        public string ImportType { get; set; }        
        public string? SelectedDate { get; set; }        

    }
}
