

using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ShoppingPageDetails
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SubCategory>? SubCategories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
