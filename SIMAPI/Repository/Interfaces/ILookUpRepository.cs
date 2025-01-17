using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface ILookUpRepository : IRepository
    {
        Task<IEnumerable<LookupResult>> GetAreaLookup(GetLookupRequest request);
        Task<IEnumerable<LookupResult>> GetShopLookup(int areaId);
        Task<IEnumerable<LookupResult>> GetNetworkLookup();
        Task<IEnumerable<LookupResult>> GetUserLookup(GetLookupRequest request);
        Task<IEnumerable<LookupResult>> GetUserRoleLookupAsync();
        Task<IEnumerable<LookupResult>> GetSupplierLookupAsync();
        Task<IEnumerable<LookupResult>> GetSupplierAccountLookupAsync(int supplierId);

        Task<IEnumerable<LookupResult>> GetCategories();
        Task<IEnumerable<LookupResult>> GetSubCategories(int categoryId);
        Task<IEnumerable<LookupResult>> GetAvailableColours();
        Task<IEnumerable<LookupResult>> GetAvailableSizes();
        Task<IEnumerable<LookupResult>> GetConfigurationTypes();
        Task<IEnumerable<LookupResult>> GetProducts();

        Task<IEnumerable<LookupResult>> GetOrderStatusTypes();
        Task<IEnumerable<LookupResult>> GetOrderPaymentTypes();
        Task<IEnumerable<LookupResult>> GetOrderDeliveryTypes();
    }
}
