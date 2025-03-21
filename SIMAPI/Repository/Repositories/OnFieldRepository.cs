﻿using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.OnField;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class OnFieldRepository : Repository, IOnFieldRepository
    {
        public OnFieldRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OnFieldCommissionModel>> OnFieldCommissionListAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
               new SqlParameter("@shopId", request.shopId),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@fromDate",request.fromDate),
                new SqlParameter("@toDate", request.toDate)
            };
            return await ExecuteStoredProcedureAsync<OnFieldCommissionModel>("exec [dbo].[OnField_Commission] @shopId, @userId, @fromDate, @toDate", sqlParameters);
        }

        public async Task<IEnumerable<OnFieldActivationModel>> OnFieldActivationListAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@shopId", request.shopId),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@fromDate",request.fromDate),
                new SqlParameter("@toDate", request.toDate),
                new SqlParameter("@isInstantActivation", request.isInstantActivation)
            };
            return await ExecuteStoredProcedureAsync<OnFieldActivationModel>("exec [dbo].[OnField_Activation] @shopId, @userId, @fromDate, @toDate, @isInstantActivation", sqlParameters);
        }

        public async Task<List<dynamic>> OnFieldGivenVSActivationListync(GetReportRequest request)
        {
            var sqlParameters = new[]
             {
                new SqlParameter("@shopId", request.shopId),
                new SqlParameter("@fromDate",request.fromDate),
                new SqlParameter("@toDate", request.toDate),
            };
            return await GetDataSet("OnField_GivenVsActivations", sqlParameters);
            //return await ExecuteStoredProcedureAsync<OnFieldGivenVsActivation>("exec [dbo].[OnField_GivenVsActivations] @shopId, @fromDate, @toDate", sqlParameters);
        }

        public async Task<IEnumerable<ShopVisitHistoryModel>> OnFieldShopVisitHistoryAsync(int shopId)
        {
            var sqlParameters = new[]
             {
                new SqlParameter("@shopId", shopId),
            };
            return await ExecuteStoredProcedureAsync<ShopVisitHistoryModel>("exec [dbo].[OnField_ShopVisit_History] @shopId", sqlParameters);
        }

        public async Task<ShopWalletAmountModel> OnFieildCommissionWalletAmountsAsync(int shopId)
        {
            var sqlParameters = new[]
             {
                new SqlParameter("@shopId", shopId),
            };
            var result = await ExecuteStoredProcedureAsync<ShopWalletAmountModel>("exec [dbo].[OnField_Commission_Wallet_Amount] @shopId", sqlParameters);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<ShopWalletHistoryModel>> OnFieldCommissionWalletHistoryAsync(int shopId, string walletType)
        {
            var sqlParameters = new[]
             {
                new SqlParameter("@shopId", shopId),
                new SqlParameter("@walletType",walletType)
            };
            return await ExecuteStoredProcedureAsync<ShopWalletHistoryModel>("exec [dbo].[OnField_Commission_Wallet_History] @shopId, @walletType", sqlParameters);
        }
    }
}
