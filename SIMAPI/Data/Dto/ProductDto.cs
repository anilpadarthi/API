using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SIMAPI.Data.Dto
{
    public partial class ProductDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ProductCode { get; set; }

        public int? CategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        public string? Description { get; set; }

        public string? Specification { get; set; }

        public bool? IsNewArrival { get; set; }

        public bool? IsBundle { get; set; }
        public bool? IsOutOfStock { get; set; }
        public int? MixMatchGroupId { get; set; }

        public bool? IsVatEnabled { get; set; }
        public int DisplayOrder { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? CommissionToAgent { get; set; }
        public decimal? CommissionToManager { get; set; }
        public IFormFile? ProductImageFile { get; set; }
        public short? Status { get; set; }
        public List<int>? SizeList { get; set; }
        public List<int>? ColourList { get; set; }
        public List<ProductPriceDto>? ProductPrices { get; set; }

    }
}