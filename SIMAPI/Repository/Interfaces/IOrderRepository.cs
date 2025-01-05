using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.OrderListModels;

namespace SIMAPI.Repository.Interfaces
{
    public interface IOrderRepository: IRepository
    {
        
       
        Task<Order> GetByIdAsync(int id);
        Task<EditOrderDetailsModel> GetOrderDetailsByIdAsync(int orderId);
        Task<IEnumerable<OrderListViewModel>> GetPagedOrderListAsync(GetPagedOrderListRequest request);
        Task<int> GetTotalCountAsync(GetPagedSearch request);
        Task<IEnumerable<OrderItemViewModel>> GetOrderItemsAsync(int orderId);
        Task<IEnumerable<OrderDetail>> GetItemsAsync(int orderId);
        Task<OrderDetail> GetOrderDetailAsync(int orderDetailId);
        Task<IEnumerable<OrderDetail>> GetPagedOrderDetailsAsync(int orderId);
        Task<IEnumerable<OrderHistoryViewModel>> GetOrderHistoryAsync(int orderId);
        Task<OrderHistory> GetOrderHistoryDetailsAsync(int orderHistoryId);
        Task<IEnumerable<OrderHistory>> GetPagedOrderHistoryDetailsAsync(int orderId);
        Task<IEnumerable<OrderPayment>> GetOrderPaymentsAsync(int orderId);
        Task<OrderPayment> GetOrderPaymentDetailsAsync(int orderPaymentDetailId);
        Task<IEnumerable<OrderPayment>> GetPagedOrderPaymentsAsync(int orderId);
        Task<int> GetOrderNotificationCountAsync();

    }
}
