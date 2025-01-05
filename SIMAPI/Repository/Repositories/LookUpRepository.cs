using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class LookUpRepository : Repository, ILookUpRepository
    {
        public LookUpRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LookupResult>> GetAreaLookup(GetLookupRequest request)
        {
            if (request.userRoleId == (int)EnumUserRole.Manager)
            {
                return await (from a in _context.Set<Area>()
                              join b in _context.Set<AreaMap>()
                              on a.AreaId equals b.AreaId into temp1
                              from t1 in temp1
                              join c in _context.Set<UserMap>()
                              on t1.UserId equals c.UserId into temp2
                              from t2 in temp2.DefaultIfEmpty()
                              where a.Status == (short)EnumStatus.Active && t1.IsActive == true && t2.IsActive == true
                              && (t1.UserId == request.userId || t2.AssignedToUserId == request.userId)
                              select new LookupResult
                              {
                                  Id = a.AreaId,
                                  Name = a.AreaName
                              }).OrderBy(o => o.Name).ToListAsync();
            }
            else if (request.userRoleId == (int)EnumUserRole.Agent)
            {
                return await (from a in _context.Set<Area>()
                              join b in _context.Set<AreaMap>()
                              on a.AreaId equals b.AreaId
                              where a.Status == (short)EnumStatus.Active && b.IsActive == true
                              && b.UserId == request.userId
                              select new LookupResult
                              {
                                  Id = a.AreaId,
                                  Name = a.AreaName
                              }).OrderBy(o => o.Name).ToListAsync();
            }
            else
            {
                return await _context.Set<Area>()
                             .Where(w => w.Status == (short)EnumStatus.Active)
                             .Select(x => new LookupResult
                             {
                                 Id = x.AreaId,
                                 Name = x.AreaName
                             }).OrderBy(o => o.Name).ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupResult>> GetNetworkLookup()
        {
            var resultList = await _context.Set<BaseNetwork>()
                              .Where(w => w.IsActive == true)
                              .Select(x => new LookupResult
                              {
                                  Id = x.BaseNetworkId,
                                  Name = x.BaseNetworkName
                              }).OrderBy(o => o.Name).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetShopLookup(int areaId)
        {
            var resultList = await _context.Set<Shop>()
                             .Where(w => w.AreaId == areaId && w.Status == 1)
                             .Select(x => new LookupResult
                             {
                                 Id = x.ShopId,
                                 Name = x.ShopName
                             }).OrderBy(o => o.Name).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetUserLookup(GetLookupRequest request)
        {
            if (request.filterType == "Managers")
            {
                return await _context.Set<User>()
                                 .Where(w => w.Status == (int)EnumStatus.Active && w.UserRoleId == (int)EnumUserRole.Manager)
                                 .Select(x => new LookupResult
                                 {
                                     Id = x.UserId,
                                     Name = x.UserName
                                 }).OrderBy(o => o.Name).ToListAsync();
            }
            else
            {
                if (request.userRoleId == (int)EnumUserRole.Manager)
                {
                    return await (from a in _context.Set<User>()
                                  join b in _context.Set<UserMap>()
                                  on a.UserId equals b.UserId
                                  where a.Status == (int)EnumStatus.Active && b.IsActive == true
                                  && b.AssignedToUserId == request.userId
                                  select new LookupResult
                                  {
                                      Id = a.UserId,
                                      Name = a.UserName
                                  }).OrderBy(o => o.Name).ToListAsync();
                }
                else
                {
                    return await _context.Set<User>()
                                 .Where(w => w.Status == (int)EnumStatus.Active)
                                 .Select(x => new LookupResult
                                 {
                                     Id = x.UserId,
                                     Name = x.UserName
                                 }).OrderBy(o => o.Name).ToListAsync();
                }
            }
        }

        public async Task<IEnumerable<LookupResult>> GetUserRoleLookupAsync()
        {
            var resultList = await _context.Set<UserRole>()
                              .Select(x => new LookupResult
                              {
                                  Id = x.UserRoleId,
                                  Name = x.RoleName
                              }).OrderBy(o => o.Name).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetSupplierLookupAsync()
        {
            var resultList = await _context.Set<Supplier>()
                              .Where(w => w.Status == (int)EnumStatus.Active)
                              .Select(x => new LookupResult
                              {
                                  Id = x.SupplierId,
                                  Name = x.SupplierName
                              }).OrderBy(o => o.Name).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetSupplierAccountLookupAsync(int supplierId)
        {
            var resultList = await _context.Set<SupplierAccount>()
                              .Where(w => w.Status == (int)EnumStatus.Active && w.SupplierId == supplierId)
                              .Select(x => new LookupResult
                              {
                                  Id = x.SupplierAccountId,
                                  Name = x.AccountName
                              }).OrderBy(o => o.Name).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetCategories()
        {
            var resultList = await _context.Set<Category>()
                             .Where(w => w.Status == (int)EnumStatus.Active)
                             .Select(x => new LookupResult
                             {
                                 Id = x.CategoryId,
                                 Name = x.CategoryName
                             }).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetSubCategories(int categoryId)
        {
            var resultList = await _context.Set<SubCategory>()
                             .Where(w => w.CategoryId == categoryId)
                             .Where(w => w.Status == (int)EnumStatus.Active)
                             .Select(x => new LookupResult
                             {
                                 Id = x.SubCategoryId,
                                 Name = x.SubCategoryName
                             }).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetAvailableColours()
        {
            var resultList = await _context.Set<Colour>()
                             .Select(x => new LookupResult
                             {
                                 Id = x.ColourId,
                                 Name = x.ColourName
                             }).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetAvailableSizes()
        {
            var resultList = await _context.Set<Size>()
                             .Select(x => new LookupResult
                             {
                                 Id = x.SizeId,
                                 Name = x.Name
                             }).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetConfigurationTypes()
        {
            var resultList = await _context.Set<ConfigurationType>()
                             .Select(x => new LookupResult
                             {
                                 Id = x.ConfigurationTypeId,
                                 Name = x.Name
                             }).ToListAsync();

            return resultList;
        }

        public async Task<IEnumerable<LookupResult>> GetProducts()
        {
            var resultList = await _context.Set<Product>()
                             .Select(x => new LookupResult
                             {
                                 Id = x.ProductId,
                                 Name = x.ProductName
                             }).ToListAsync();

            return resultList;
        }
    }
}
