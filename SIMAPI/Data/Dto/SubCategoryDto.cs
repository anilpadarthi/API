namespace SIMAPI.Data.Dto
{
    public partial class SubCategoryDto
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int DisplayOrder { get; set; }
        public short Status { get; set; }

    }
}
