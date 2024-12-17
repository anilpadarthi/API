using SIMAPI.Data.Entities;
using AutoMapper;
using SIMAPI.Data.Dto;

namespace SIMAPI.Business.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserDocument, UserDocumentDto>().ReverseMap();
            CreateMap<Area, AreaDto>().ReverseMap();
            CreateMap<AreaLog, Area>().ReverseMap();
            CreateMap<Network, NetworkDto>().ReverseMap();
            CreateMap<Shop, ShopDto>().ReverseMap();
            CreateMap<ShopContact, ShopContactDto>().ReverseMap();
            CreateMap<ShopLog, Shop>().ReverseMap();
            CreateMap<UserTrack, UserTrackDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>().ReverseMap();
        }
    }
}
