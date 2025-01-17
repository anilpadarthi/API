
namespace SIMAPI.Data.Dto
{
    public class ShopDto
    {
        public int ShopId { get; set; }
        public int AreaId { get; set; }
        public string ShopName { get; set; }
        public string PostCode { get; set; }
        public string? VatNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Competitor { get; set; }
        public string? Language { get; set; }
        public string? City { get; set; }
        public string? PaymentMode { get; set; }
        public string? PayableName { get; set; }
        public string? DeliveryInstructions { get; set; }
        public string? Comments { get; set; }
        public string? TopupSystemId { get; set; }
        public bool? IsMobileShop { get; set; }
        public short Status { get; set; }
        public IFormFile? ImageFile { get; set; }
        public bool? IsTermsAndCondtions { get; set; }       
        public DateTime? AgreementFrom { get; set; }
        public DateTime? AgreementTo { get; set; }
        public string? AgreementNotes { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public ShopContactDto[]? ShopContacts { get; set; }

    }
}
