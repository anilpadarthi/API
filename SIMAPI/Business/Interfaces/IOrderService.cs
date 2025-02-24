using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IOrderService
    {
        Task<CommonResponse> CreateAsync(OrderDetailDto request);
        Task<CommonResponse> CreateOrderPaymentAsync(OrderPaymentDto request);
        Task<CommonResponse> UpdateOrderPaymentAsync(int orderPaymentId);
        Task<CommonResponse> DeleteOrderPaymentAsync(int orderPaymentId);
        Task<CommonResponse> UpdateAsync(OrderDetailDto request);
        Task<CommonResponse> UpdateOrderDetailsAsync(OrderStatusModel request);
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetShoppingPageDetailsAsync();
        Task<CommonResponse> GetPagedOrderListAsync(GetPagedOrderListDto request);
        Task<CommonResponse> GetOrderHistoryAsync(int orderId);
        Task<CommonResponse> GetOrderPaymentHistoryAsync(int orderId);
        Task<CommonResponse> DownloadOrderListAsync(GetPagedOrderListDto request);
        Task<CommonResponse> GetOrderNotificationCountAsync();



        //Task<IEnumerable<string>> CreateOrderAsync(Order request);
        //Task<IEnumerable<string>> UpdateOrderAsync(Order request);
        //Task<IEnumerable<string>> UpdateOrderDetailsAsync(Order request);
        //Task<IEnumerable<string>> DeleteOrderAsync(int orderId);
        //Task<Order> GetOrderAsync(int orderId);
        //Task<IEnumerable<Order>> GetAllOrdersAsync();
        //Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        //Task<IEnumerable<OrderDetailsMap>> GetOrderDetailsAsync(int orderId);
        //Task<OrderDetailsMap> GetOrderDetailAsync(int orderDetailId);
        //Task<IEnumerable<OrderDetailsMap>> GetPagedOrderDetailsAsync(int orderId);

        //Task<OrderHistoryMap> GetOrderHistoryDetailsAsync(int orderHistoryId);
        //Task<IEnumerable<OrderHistoryMap>> GetPagedOrderHistoryDetailsAsync(int orderId);
        //Task<IEnumerable<OrderPaymentMap>> GetOrderPaymentsAsync(int orderId);
        //Task<OrderPaymentMap> GetOrderPaymentDetailsAsync(int orderPaymentDetailId);
        //Task<IEnumerable<OrderPaymentMap>> GetPagedOrderPaymentsAsync(int orderId);
        //Task<IEnumerable<string>> CreateOrderHistoryAsync(OrderHistoryMap request);
        //Task<IEnumerable<string>> UpdateOrderHistoryAsync(OrderHistoryMap request);
        //Task<IEnumerable<string>> DeleteOrderHistoryAsync(int orderHistoryId);

        //Task<IEnumerable<string>> CreateOrderPaymentAsync(OrderPaymentMap request);
        //Task<IEnumerable<string>> UpdateOrderPaymentAsync(OrderPaymentMap request);
        //Task<IEnumerable<string>> DeleteOrderPaymentAsync(int orderPaymentId);
    }
}
