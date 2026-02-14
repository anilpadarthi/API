using AutoMapper;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Export;
using SIMAPI.Data.Models.Tracking;

namespace SIMAPI.Business.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Attendance, AttendanceDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserLog, User>().ReverseMap();
            CreateMap<UserDocument, UserDocumentDto>().ReverseMap();
            CreateMap<Area, AreaDto>().ReverseMap();
            CreateMap<AreaLog, Area>().ReverseMap();
            CreateMap<Network, NetworkDto>().ReverseMap();
            CreateMap<Shop, ShopDto>().ReverseMap();
            CreateMap<ShopContact, ShopContactDto>().ReverseMap();
            CreateMap<ShopLog, Shop>().ReverseMap();
            CreateMap<UserTrack, UserTrackDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
            CreateMap<SupplierTransaction, SupplierTransactionDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<SubCategory, SubCategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductPrice, ProductPriceDto>().ReverseMap();
            CreateMap<OrderPayment, OrderPaymentDto>().ReverseMap();
            CreateMap<VwShops, ExportShop>().ReverseMap();
            CreateMap<VwOrders, ExportSaleOrder>().ReverseMap();
            CreateMap<PurchaseInvoice, PurchaseInvoiceCreateDto>().ReverseMap();
            CreateMap<PurchaseInvoiceItem, PurchaseInvoiceItemCreateDto>().ReverseMap();
        }
    }
}
