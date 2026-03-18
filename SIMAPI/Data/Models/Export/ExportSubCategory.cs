
namespace SIMAPI.Data.Models.Export
{
    public class ExportSubCategory
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
        public short? Status { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
