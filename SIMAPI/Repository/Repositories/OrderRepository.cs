using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OrderListModels;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class OrderRepository : Repository, IOrderRepository
    {
        public OrderRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<ShoppingPageDetails> GetShoppingPageDetailsAsync()
        {
            ShoppingPageDetails shoppingPageDetails = new ShoppingPageDetails();
            shoppingPageDetails.Categories = await _context.Set<Category>().Include(i => i.SubCategories).Where(w => w.Status == 1).ToListAsync();
            shoppingPageDetails.Products = await _context.Set<Product>().Include(i => i.ProductPrices).Where(w => w.Status == 1).ToListAsync();

            return shoppingPageDetails;
        }

        public async Task<IEnumerable<VwOrders>> GetOrdersByPagingAsync(GetPagedOrderListDto request)
        {

            var query = GetVwOrdersQuery(request);

            var result = await query
                .OrderByDescending(o => o.CreatedDate)
                .Skip((request.pageNo.Value - 1) * request.pageSize.Value)
                .Take(request.pageSize.Value)
                .ToListAsync();

            return result;



            //var sqlParameters = new[]
            //{

            //    request.requestType!=null ? new SqlParameter("@requestType", request.requestType) : new SqlParameter("@requestType", DBNull.Value),
            //    request.pageNo.HasValue ? new SqlParameter("@pageNo", request.pageNo.Value) : new SqlParameter("@pageNo", DBNull.Value),
            //    request.pageSize.HasValue ? new SqlParameter("@pageSize", request.pageSize.Value) : new SqlParameter("@pageSize", DBNull.Value),
            //    request.orderStatusId.HasValue ? new SqlParameter("@orderStatusId", request.orderStatusId.Value) : new SqlParameter("@orderStatusId", DBNull.Value),
            //    request.paymentMethodId.HasValue ? new SqlParameter("@paymentMethodId", request.paymentMethodId.Value) : new SqlParameter("@paymentMethodId", DBNull.Value),
            //    request.shippingModeId.HasValue ? new SqlParameter("@shippingModeId", request.shippingModeId.Value) : new SqlParameter("@shippingModeId", DBNull.Value),
            //    request.agentId.HasValue ? new SqlParameter("@agentId", request.agentId.Value) : new SqlParameter("@agentId", DBNull.Value),
            //    request.managerId.HasValue ? new SqlParameter("@managerId", request.managerId.Value) : new SqlParameter("@managerId", DBNull.Value),
            //    request.areaId.HasValue ? new SqlParameter("@areaId", request.areaId.Value) : new SqlParameter("@areaId", DBNull.Value),
            //    request.shopId.HasValue ? new SqlParameter("@shopId", request.shopId.Value) : new SqlParameter("@shopId", DBNull.Value),
            //    request.fromDate!=null ? new SqlParameter("@fromDate", request.fromDate) : new SqlParameter("@fromDate", DBNull.Value),
            //    request.toDate!=null ? new SqlParameter("@toDate", request.toDate) : new SqlParameter("@toDate", DBNull.Value),
            //    request.orderId.HasValue ? new SqlParameter("@orderId", request.orderId.Value) : new SqlParameter("@orderId", DBNull.Value),
            //    request.isVat.HasValue ? new SqlParameter("@isVat", request.isVat.Value) : new SqlParameter("@isVat", DBNull.Value),
            //    request.trackingNumber!=null ? new SqlParameter("@trackingNumber", request.trackingNumber) : new SqlParameter("@trackingNumber", DBNull.Value),
            //    request.loggedInUserRoleId.HasValue ? new SqlParameter("@loggedInUserRoleId", request.loggedInUserRoleId.Value) : new SqlParameter("@loggedInUserRoleId", DBNull.Value),
            //    request.loggedInUserId.HasValue ? new SqlParameter("@loggedInUserId", request.loggedInUserId.Value) : new SqlParameter("@loggedInUserId", DBNull.Value),
            //};
            //return await ExecuteStoredProcedureAsync<OrderListViewModel>("exec Get_Order_List @requestType, @pageNo, @pageSize, @orderStatusId," +
            //    " @paymentMethodId, @shippingModeId,@agentId,@managerId,@areaId,@shopId,@fromDate,@toDate, @orderId,@isVat," +
            //    " @loggedInUserRoleId, @loggedInUserId ", sqlParameters);
        }

        public async Task<int> GetTotalOrdersCountAsync(GetPagedOrderListDto request)
        {
            var query = GetVwOrdersQuery(request);

            return await query.CountAsync();
        }

        public async Task<OrderDetailsModel> GetOrderDetailsByIdAsync(int orderId)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@orderId", orderId)
            };
            return (await ExecuteStoredProcedureAsync<OrderDetailsModel>("exec Get_Order_Details @orderId", sqlParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<OrderItemModel>> GetOrderItemsAsync(int orderId)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@orderId", orderId)
            };
            return await ExecuteStoredProcedureAsync<OrderItemModel>("exec Get_Order_Items @orderId", sqlParameters);
        }

        public async Task<IEnumerable<OrderDetail>> GetItemsAsync(int orderId)
        {
            var result = await _context.Set<OrderDetail>().Where(w => w.OrderId == orderId && w.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<VwOrderHistory>> GetOrderHistoryAsync(int orderId)
        {
            return await _context.Set<VwOrderHistory>()
                 .Where(w => w.OrderId == orderId)
                 .OrderByDescending(w => w.CreatedDate)
                 .ToListAsync();
        }

        public async Task<IEnumerable<VwOrderPaymentHistory>> GetOrderPaymentHistoryAsync(int orderId)
        {
            return await _context.Set<VwOrderPaymentHistory>()
                 .Where(w => w.OrderId == orderId)
                 .OrderByDescending(w => w.PaymentDate)
                 .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Set<Order>()
                .Where(w => w.OrderId == id)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Order>> GetByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Order>();


            var result = await query
                .OrderByDescending(o => o.CreatedDate)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Order>();

            return await query.CountAsync();
        }



        public async Task<IEnumerable<OrderPayment>> GetOrderPaymentsAsync(int orderId)
        {
            var result = await _context.Set<OrderPayment>().Where(w => w.OrderId == orderId).ToListAsync();
            return result;
        }

        public async Task<OrderDetail> GetOrderDetailAsync(int orderDetailId)
        {
            var result = await _context.Set<OrderDetail>().Where(w => w.OrderId == orderDetailId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<OrderHistory> GetOrderHistoryDetailsAsync(int orderHistoryId)
        {
            var result = await _context.Set<OrderHistory>().Where(w => w.OrderId == orderHistoryId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<OrderPayment> GetOrderPaymentDetailsAsync(int orderPaymentDetailId)
        {
            var result = await _context.Set<OrderPayment>().Where(w => w.OrderPaymentId == orderPaymentDetailId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<OrderDetail>> GetPagedOrderDetailsAsync(int orderId)
        {
            var result = await _context.Set<OrderDetail>().Where(w => w.OrderId == orderId).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<OrderHistory>> GetPagedOrderHistoryDetailsAsync(int orderId)
        {
            var result = await _context.Set<OrderHistory>().Where(w => w.OrderId == orderId).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<OrderPayment>> GetPagedOrderPaymentsAsync(int orderId)
        {
            var result = await _context.Set<OrderPayment>().Where(w => w.OrderId == orderId).ToListAsync();
            return result;
        }

        public async Task<int> GetOrderNotificationCountAsync()
        {
            var result = await _context.Set<Order>().Where(w => w.IsRead == 0).CountAsync();
            return result;
        }


        private IQueryable<VwOrders> GetVwOrdersQuery(GetPagedOrderListDto request)
        {
            var query = _context.Set<VwOrders>().AsQueryable();

            if (request.orderStatusId.HasValue)
            {
                query = query.Where(w => w.OrderStatusId == request.orderStatusId.Value);
            }

            if (request.paymentMethodId.HasValue)
            {
                query = query.Where(w => w.PaymentMethodId == request.paymentMethodId.Value);
            }

            if (request.shippingModeId.HasValue)
            {
                query = query.Where(w => w.shippingModeId == request.shippingModeId.Value);
            }

            if (request.areaId.HasValue)
            {
                query = query.Where(w => w.AreaId == request.areaId.Value);
            }

            if (request.shopId.HasValue)
            {
                query = query.Where(w => w.ShopId == request.shopId.Value);
            }

            if (request.agentId.HasValue)
            {
                query = query.Where(w => w.UserId == request.agentId.Value);
            }

            if (request.orderId.HasValue)
            {
                query = query.Where(w => w.OrderId == request.orderId.Value);
            }

            if (!string.IsNullOrEmpty(request.trackingNumber))
            {
                query = query.Where(w => w.TrackingNumber == request.trackingNumber);
            }

            if (request.isVat.HasValue && request.isVat.Value == 1)
            {
                query = query.Where(w => w.IsVATApplicable == request.isVat.Value);
            }
            //if (!string.IsNullOrEmpty(request.fromDate))
            if (request.fromDate.HasValue)
            {
                query = query.Where(w => w.CreatedDate.Value >= request.fromDate.Value);
            }

            //if (!string.IsNullOrEmpty(request.toDate))
            if (request.toDate.HasValue)
            {
                query = query.Where(w => w.CreatedDate.Value <= request.toDate.Value);
            }

            return query;
        }
    }
}
