namespace SIMAPI.Data.Dto
{
    public partial class ProductSearchModel
    {
        public string? searchText { get; set; }
        public int? categoryId { get; set; }
        public int? subCategoryId { get; set; }
    }
}