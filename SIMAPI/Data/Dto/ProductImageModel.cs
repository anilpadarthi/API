using Microsoft.AspNetCore.Http;

namespace SIMAPI.Data.Dto
{
    public class ProductImageModel
    {
        public int ProductId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
