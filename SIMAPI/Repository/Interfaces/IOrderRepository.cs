﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OrderListModels;

namespace SIMAPI.Repository.Interfaces
{
    public interface IOrderRepository: IRepository
    {        
       
        Task<OrderInfo> GetByIdAsync(int id);
        Task<OrderDetailsModel> GetOrderDetailsByIdAsync(int orderId);
        Task<InvoiceDetailModel> GetOrderDetailsForInvoiceByIdAsync(int orderId);
        Task<OutstandingAmountModel?> LoadOutstandingMetricsAsync(string filterType, int filterId);
        Task<ShoppingPageDetails> GetShoppingPageDetailsAsync();
        Task<IEnumerable<VwOrders>> GetOrdersByPagingAsync(GetPagedOrderListDto request);
        Task<int> GetTotalOrdersCountAsync(GetPagedOrderListDto request);
        Task<int> GetTotalCountAsync(GetPagedSearch request);
        Task<IEnumerable<OrderItemModel>> GetOrderItemsAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetItemsAsync(int orderId);
        Task<OrderDetail> GetOrderDetailAsync(int orderDetailId);
        Task<IEnumerable<OrderDetail>> GetPagedOrderDetailsAsync(int orderId);
        Task<IEnumerable<VwOrderHistory>> GetOrderHistoryAsync(int orderId);
        Task<IEnumerable<VwOrderPaymentHistory>> GetOrderPaymentHistoryAsync(int orderId);
        Task<OrderHistory> GetOrderHistoryDetailsAsync(int orderHistoryId);
        Task<IEnumerable<OrderHistory>> GetPagedOrderHistoryDetailsAsync(int orderId);
        Task<IEnumerable<OrderPayment>> GetOrderPaymentsAsync(int orderId);
        Task<OrderPayment> GetOrderPaymentDetailsAsync(int orderPaymentDetailId);
        Task<IEnumerable<OrderPayment>> GetPagedOrderPaymentsAsync(int orderId);
        Task<int> GetOrderNotificationCountAsync();
        Task<IEnumerable<ShopWalletHistory>> GetShopWalletHistoryByReferenceNumber(string referenceNumber,string transactionType);

    }
}
