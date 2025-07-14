namespace SIMAPI.Data.Dto
{
    public partial class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal? CommissionPercent { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int DisplayOrder { get; set; }
        public short Status { get; set; }

    }
}
