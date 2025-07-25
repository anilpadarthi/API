﻿using Microsoft.Data.SqlClient;
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

        public async Task<IEnumerable<Shop>> GetAllShopsAsync(int? areaId)
        {
            return await _context.Set<Shop>()
                .Where(w => w.Status == 1 && w.AreaId == areaId)
                .ToListAsync();
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
                query = query.Where(w => w.ShopName.Contains(request.searchText));
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


    }
}
