using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OnField;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class ShopRepository : Repository, IShopRepository
    {
        public ShopRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<int?> GetNextOldShopIdAsync()
        {
            return await _context.Set<Shop>()
                .MaxAsync(w => w.OldShopId);
        }
        public async Task<IEnumerable<VwShops>> GetAllShopsAsync(int? areaId)
        {
            if (areaId == null || areaId == 0)
            {
                return await _context.Set<VwShops>()
                .ToListAsync();
            }
            else
            {
                return await _context.Set<VwShops>()
                    .Where(w => w.AreaId == areaId)
                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<ShopContact>> GetShopContactsAsync(int shopId)
        {
            return await _context.Set<ShopContact>()
                                        .Where(w => w.ShopId == shopId && w.Status == (int)EnumStatus.Active)
                                        .OrderBy(o => o.ShopContactId)
                                        .ToListAsync();
        }

        public async Task<ShopAgreement> GetShopAgreementAsync(int shopId)
        {
            return await _context.Set<ShopAgreement>()
                                        .Where(w => w.ShopId == shopId && w.Status != (int)EnumStatus.InActive)
                                        .OrderByDescending(o => o.CreatedDate)
                                        .FirstOrDefaultAsync();
        }

        public async Task<ShopDetails> GetShopDetailsAsync(int shopId)
        {
            ShopDetails shopDetails = new ShopDetails();
            shopDetails.shop = await _context.Set<Shop>()
                .Where(w => w.ShopId == shopId)
                .FirstOrDefaultAsync();
            shopDetails.shopAgreement = await _context.Set<ShopAgreement>()
                                       .Where(w => w.ShopId == shopId && w.Status != (int)EnumStatus.InActive)
                                       .OrderByDescending(o => o.CreatedDate)
                                       .FirstOrDefaultAsync();
            shopDetails.shopContacts = await _context.Set<ShopContact>()
                                        .Where(w => w.ShopId == shopId && w.Status == (int)EnumStatus.Active)
                                        .OrderBy(o => o.ShopContactId)
                                        .ToListAsync();

            return shopDetails;
        }

        public async Task<Shop> GetShopByIdAsync(int shopId)
        {
            return await _context.Set<Shop>()
                 .Where(w => w.ShopId == shopId)
                 .FirstOrDefaultAsync();

        }

        public async Task<Shop> GetShopByNameAsync(string name, string postCode)
        {
            return await _context.Set<Shop>()
                .Where(w => w.ShopName.Trim().ToUpper() == name.Trim().ToUpper() && w.PostCode.Trim().ToUpper() == postCode.Trim().ToUpper())
                .Where(w => w.Status == 1)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Shop>> GetShopsByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Shop>()
                 .Include(i => i.Area)
                 .AsQueryable();

            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (request.areaId.HasValue)
            {
                query = query.Where(w => w.AreaId == request.areaId);
            }

            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int shopId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.ShopId == shopId);
                }
                else
                {
                    // Otherwise → search by name
                    query = query.Where(w => w.ShopName.Contains(request.searchText));
                }
            }

            var result = await query
                .OrderBy(o => o.ShopName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalShopsCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Shop>().AsQueryable();

            if (request.areaId.HasValue)
            {
                query = query.Where(w => w.AreaId == request.areaId);
            }

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.ShopName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }


        public async Task<bool> ShopVisitAsync(ShopVisitRequestmodel request)
        {
            ShopVisit shopVisit = new ShopVisit();
            shopVisit.ShopId = request.ShopId;
            shopVisit.UserId = request.UserId.Value;
            shopVisit.Comment = request.Comments;
            shopVisit.ReferenceImage = request.ReferenceImage;
            shopVisit.IsSentToWhatsApp = 0;
            shopVisit.CreatedDate = DateTime.Now;
            _context.Add(shopVisit);

            UserTrack userTrack = new UserTrack();
            userTrack.ShopId = request.ShopId;
            userTrack.UserId = request.UserId.Value;
            userTrack.TrackedDate = DateTime.Now;
            userTrack.CreatedDate = DateTime.Now;
            userTrack.WorkType = "ShopVisit";
            userTrack.Latitude = request.Latitude;
            userTrack.Longitude = request.Longitude;
            userTrack.Comments = request.Comments;
            _context.Add(userTrack);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ShopVisitHistoryModel>> GetShopVisitHistoryAsync(int shopId)
        {
            var paramList = new[]
            {
                    new SqlParameter("@shopId", shopId)
            };
            return await ExecuteStoredProcedureAsync<ShopVisitHistoryModel>("exec [dbo].[OnField_ShopVisit_History] @shopId", paramList);
        }

        public async Task<IEnumerable<ShopAgreementHistoryModel>> GetShopAgreementHistoryAsync(int shopId)
        {
            var paramList = new[]
            {
                    new SqlParameter("@shopId", shopId)
            };
            return await ExecuteStoredProcedureAsync<ShopAgreementHistoryModel>("exec [dbo].[ShopAgreement_History] @shopId", paramList);
        }

        public async Task<ShopWalletAmountModel> GetShopWalletAmountAsync(int shopId)
        {
            var paramList = new[]
            {
                    new SqlParameter("@shopId", shopId)
            };
            return await ExecuteStoredProcedureReturnsFirstItemAsync<ShopWalletAmountModel>("exec [dbo].[OnField_Commission_Wallet_Amount] @shopId", paramList);
        }

        public async Task<IEnumerable<ShopWalletHistoryModel>> GetShopWalletHistoryAsync(int shopId, string walletType)
        {
            var paramList = new[]
            {
                    new SqlParameter("@shopId", shopId),
                    new SqlParameter("@walletType", walletType)
            };
            return await ExecuteStoredProcedureAsync<ShopWalletHistoryModel>("exec [dbo].[OnField_Commission_Wallet_History] @shopId,@walletType", paramList);
        }

        public async Task<ShopAddressDetails?> GetShopAddressDetailsAsync(int shopId)
        {
            return await _context.Set<Shop>()
                .Where(w => w.ShopId == shopId)
                             .Select(x => new ShopAddressDetails
                             {
                                 ShopId = x.ShopId,
                                 ShopName = x.ShopName,
                                 PostCode = x.PostCode,
                                 AddressLine1 = x.AddressLine1,
                                 Latitude = x.Latitude,
                                 Longitude = x.Longitude,
                             }).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<ShopCommissionChequeDto>> GetShopCommissionChequesAsync(int shopId, string mode)
        {
            var paramList = new[]
            {
                    new SqlParameter("@mode", mode),
                    new SqlParameter("@shopId", shopId),
            };
            return await ExecuteStoredProcedureAsync<ShopCommissionChequeDto>("exec [dbo].[ManageBankCheques] @mode,@shopId", paramList);
        }

        public async Task<ShopCommissionCheques> GetShopCommissionChequeAsync(int sno)
        {
            return await _context.Set<ShopCommissionCheques>()
                 .Where(w => w.Sno == sno)
                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VwShops>> GlobalShopSearchAsync(GetLookupRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.searchText))
                return Enumerable.Empty<VwShops>();

            string normalized = request.searchText.Trim().ToUpper();
            string status = Convert.ToString((short)EnumStatus.Active);
            bool isNumeric = int.TryParse(request.searchText, out int shopId);
            if (request.userRoleId == (int)EnumUserRole.Manager)
            {
                return await (from s in _context.Set<VwShops>()                              
                              join c in _context.Set<UserMap>()
                              on s.UserId equals c.UserId into temp2
                              from t2 in temp2.DefaultIfEmpty()
                              where s.Status == status && t2.IsActive == true
                              && (s.UserId == request.userId || t2.MonitorBy == request.userId)
                              && (isNumeric
                            ? (s.OldShopId == shopId)
                            : ((s.ShopName != null && s.ShopName.ToUpper().Contains(normalized))
                               || (s.PostCode != null && s.PostCode.ToUpper().Contains(normalized))))
                              select s).ToListAsync();
            }
            else if (request.userRoleId == (int)EnumUserRole.Agent)
            {
                return await (from s in _context.Set<VwShops>()
                              where s.Status == status
                              && s.UserId == request.userId
                              && (isNumeric
                            ? (s.OldShopId == shopId)
                            : ((s.ShopName != null && s.ShopName.ToUpper().Contains(normalized))
                               || (s.PostCode != null && s.PostCode.ToUpper().Contains(normalized))))
                              select s).ToListAsync();
            }
            else if (request.userRoleId == (int)EnumUserRole.Admin
                || request.userRoleId == (int)EnumUserRole.SuperAdmin
                || request.userRoleId == (int)EnumUserRole.CallCenter)
            {
                return await _context.Set<VwShops>()
                             .Where(w => w.Status == status
                             && (isNumeric
                            ? (w.OldShopId == shopId)
                            : ((w.ShopName != null && w.ShopName.ToUpper().Contains(normalized))
                               || (w.PostCode != null && w.PostCode.ToUpper().Contains(normalized))))).ToListAsync();

            }
            else
            {
                return Enumerable.Empty<VwShops>();
            }
        }

    }
}
