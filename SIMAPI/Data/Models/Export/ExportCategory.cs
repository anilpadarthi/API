
namespace SIMAPI.Data.Models.Export
{
    public class ExportCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public short? Status { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
