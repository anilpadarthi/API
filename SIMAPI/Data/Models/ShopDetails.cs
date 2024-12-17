using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ShopDetails
    {
        public Shop shop { get; set; }
        public ShopAgreement shopAgreement { get; set; }
        public List<ShopContact> shopContacts { get; set; }
    }
}
