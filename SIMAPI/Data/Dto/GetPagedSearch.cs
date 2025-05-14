namespace SIMAPI.Data.Dto
{
    public class GetPagedSearch
    {
        public int pageNo { get; set; }
        public int pageSize { get; set; }
        public int? id { get; set; }
        public int? categoryId { get; set; }
        public int? subCategoryId { get; set; }
        public int? areaId { get; set; }
        public string? searchText { get; set; }
        public string? mode { get; set; }
        public int? loggedInUserId { get; set; }
        public int userRoleId { get; set; }
    }
}
