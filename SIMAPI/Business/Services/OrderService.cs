using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OrderListModels;
using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository OrderRepository, IMapper mapper)
        {
            _orderRepository = OrderRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(OrderDetailsModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                int orderId = 0;
                if (request != null)
                {
                    orderId = await CreateOrder(request);
                }

                foreach (var item in request.Items)
                {
                    OrderDetail mapObject = new OrderDetail()
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        SalePrice = item.SalePrice,
                        Qty = item.Qty,
                        ProductColourId = item.ProductColourId,
                        ProductSizeId = item.ProductSizeId,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1
                    };
                    _orderRepository.Add(mapObject);
                }
                await _orderRepository.SaveChangesAsync();

                await CreateHistoryRecord(request, "Created");
                response = Utility.CreateResponse("Order placed successfully", HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(OrderDetailsModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                int orderId = request.OrderId ?? 0;
                if (request != null && orderId > 0)
                {
                    await UpdateOrder(request);
                    var savedItems = (await _orderRepository.GetItemsAsync(orderId)).ToList();

                    //update existing items as inactive if not found in the requested items
                    foreach (var item in savedItems)
                    {
                        var IsSavedItem = request.Items.Where(e => e.ProductId == item.ProductId).FirstOrDefault();
                        if (IsSavedItem != null)
                        {
                            item.Qty = IsSavedItem.Qty;
                            item.SalePrice = IsSavedItem.SalePrice;
                            item.ProductSizeId = IsSavedItem.ProductSizeId;
                            item.ProductColourId = IsSavedItem.ProductColourId;
                            item.ModifiedDate = DateTime.Now;
                            item.ModifiedBy = 1;
                        }
                        else
                        {
                            item.IsActive = false;
                        }
                    }
                    await _orderRepository.SaveChangesAsync();

                    foreach (var item in request.Items)
                    {
                        var IsNewItem = savedItems.Where(e => e.ProductId == item.ProductId).FirstOrDefault();
                        if (IsNewItem == null)
                        {
                            OrderDetail mapObject = new OrderDetail()
                            {
                                OrderId = orderId,
                                ProductId = item.ProductId,
                                SalePrice = item.SalePrice,
                                Qty = item.Qty,
                                ProductColourId = item.ProductColourId,
                                ProductSizeId = item.ProductSizeId,
                                IsActive = true,
                                CreatedDate = DateTime.Now,
                                CreatedBy = 1
                            };
                            _orderRepository.Add(mapObject);
                        }
                    }
                    await _orderRepository.SaveChangesAsync();

                    await CreateHistoryRecord(request, "Updated Order Details");
                    response = Utility.CreateResponse("Order updated successfully", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }


        public async Task<CommonResponse> UpdateStatusAsync(OrderStatusModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                order.OrderStatusTypeId = request.OrderStatusId;
                order.OrderPaymentTypeId = request.PaymentMethodId;
                order.OrderDeliveryTypeId = request.ShippingModeId;
                order.TrackingNumber = request.TrackingNumber;
                order.ShippingAddress = request.ShippingAddress;
                order.ModifiedBy = 1;
                order.ModifiedDate = DateTime.Now;
                await _orderRepository.SaveChangesAsync();

                OrderDetailsModel request1 = new OrderDetailsModel();
                request1.OrderId = request.OrderId;
                request1.OrderStatusId = request.OrderStatusId;
                request1.PaymentMethodId = request.PaymentMethodId;
                request1.ShippingModeId = request.ShippingModeId;
                request1.TrackingNumber = request.TrackingNumber;
                await CreateHistoryRecord(request1, "Updated_" + request.ShippingModeId + "_" + request.TrackingNumber);
                response = Utility.CreateResponse("Updated status successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }


        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.GetOrderDetailsByIdAsync(id);
                result.Items = (await _orderRepository.GetOrderItemsAsync(id)).ToList();
                //var orderDetails = _mapper.Map<OrderDetailResponse>(result);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetPagedOrderListAsync(GetPagedOrderListRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _orderRepository.GetPagedOrderListAsync(request);
                pageResult.TotalRecords = ((OrderListViewModel)pageResult.Results.ToList().FirstOrDefault()).TotalOrdersCount ?? 0;
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetOrderHistoryAsync(int orderId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.GetOrderHistoryAsync(orderId);

                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;

        }

        public async Task<CommonResponse> DownloadOrderListAsync(GetPagedOrderListRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var orderList = await _orderRepository.GetPagedOrderListAsync(request);
                var ms = ExcelUtility.ConvertyListToMemoryStream(orderList.ToList(), "OrderList");
                response = Utility.CreateResponse(ms.ToArray(), HttpStatusCode.OK);
                //response = Utility.CreateResponse(orderList, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetOrderNotificationCountAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var totalCount = await _orderRepository.GetOrderNotificationCountAsync();
                response = Utility.CreateResponse(totalCount, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }



        //#region Order
        //public async Task<IEnumerable<string>> CreateOrderAsync(Order orderModel)
        //{
        //    _orderRepository.Add(orderModel);
        //    await _orderRepository.SaveChangesAsync();
        //    foreach (var item in orderModel.OrderDetailsMaps)
        //    {
        //        OrderDetailsMap mapObject = new OrderDetailsMap();
        //        mapObject.Order = orderModel;
        //        mapObject.ProductId = item.ProductId;
        //        mapObject.SalePrice = item.SalePrice;
        //        mapObject.Qty = item.Qty;
        //        mapObject.ProductColourId = item.ProductColourId;
        //        mapObject.ProductSizeId = item.ProductSizeId;
        //        mapObject.CreatedDate = DateTime.Now;
        //        mapObject.CreatedBy = 1;
        //        _orderRepository.Add(mapObject);
        //    }
        //    await _orderRepository.SaveChangesAsync();
        //    var historyRecord = CreateHistoryRecord(orderModel.OrderId, "Placed", orderModel.PaymentMethod);
        //    await CreateOrderHistoryAsync(historyRecord);
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Created successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<string>> DeleteOrderAsync(int orderId)
        //{
        //    var saleOrder = await _orderRepository.GetOrderAsync(orderId);
        //    saleOrder.IsActive = false;
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Deleted successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<string>> UpdateOrderAsync(Order orderModel)
        //{
        //    var saleOrder = await _orderRepository.GetOrderAsync(orderModel.OrderId);
        //    saleOrder.TotalAmount = orderModel.TotalAmount;
        //    saleOrder.NetAmount = orderModel.NetAmount;
        //    saleOrder.DiscountAmount = orderModel.DiscountAmount;
        //    saleOrder.VatAmount = orderModel.VatAmount;
        //    saleOrder.DeliveryCharges = orderModel.DeliveryCharges;
        //    saleOrder.OrderStatus = orderModel.OrderStatus;
        //    saleOrder.PaymentMethod = orderModel.PaymentMethod;
        //    saleOrder.ShippingMode = orderModel.ShippingMode;
        //    saleOrder.TrackNumber = orderModel.TrackNumber;
        //    saleOrder.IsVatEnabled = orderModel.IsVatEnabled;
        //    saleOrder.DiscountPercent = orderModel.DiscountPercent;
        //    saleOrder.VatPercent = orderModel.VatPercent;
        //    saleOrder.CouponCode = orderModel.CouponCode;
        //    saleOrder.ModifiedBy = orderModel.ModifiedBy;
        //    await _orderRepository.SaveChangesAsync();


        //    var historyRecord = CreateHistoryRecord(orderModel.OrderId, orderModel.OrderStatus, orderModel.PaymentMethod);
        //    await CreateOrderHistoryAsync(historyRecord);

        //    List<string> resultList = new List<string>();
        //    resultList.Add("Updated successfully");
        //    return resultList;
        //}

        //public async Task<Order> GetOrderAsync(int orderId)
        //{
        //    return await _orderRepository.GetOrderAsync(orderId);
        //}

        //public async Task<IEnumerable<Order>> GetPagedOrdersAsync(GetPagedSearch request)
        //{
        //    return await _orderRepository.GetPagedOrdersAsync(request);
        //}

        //public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        //{
        //    return await _orderRepository.GetAllOrdersAsync();
        //}

        //public async Task<IEnumerable<string>> UpdateOrderDetailsAsync(Order orderModel)
        //{
        //    var saleOrder = await _orderRepository.GetOrderAsync(orderModel.OrderId);
        //    saleOrder.TotalAmount = orderModel.TotalAmount;
        //    saleOrder.NetAmount = orderModel.NetAmount;
        //    saleOrder.DiscountAmount = orderModel.DiscountAmount;
        //    saleOrder.VatAmount = orderModel.VatAmount;
        //    saleOrder.DeliveryCharges = orderModel.DeliveryCharges;
        //    saleOrder.OrderStatus = orderModel.OrderStatus;
        //    saleOrder.PaymentMethod = orderModel.PaymentMethod;
        //    saleOrder.ShippingMode = orderModel.ShippingMode;
        //    saleOrder.TrackNumber = orderModel.TrackNumber;
        //    saleOrder.IsVatEnabled = orderModel.IsVatEnabled;
        //    saleOrder.DiscountPercent = orderModel.DiscountPercent;
        //    saleOrder.VatPercent = orderModel.VatPercent;
        //    saleOrder.CouponCode = orderModel.CouponCode;
        //    saleOrder.ModifiedBy = orderModel.ModifiedBy;

        //    await _orderRepository.SaveChangesAsync();
        //    //Update existing records
        //    foreach (var item in saleOrder.OrderDetailsMaps)
        //    {
        //        var isMatched = orderModel.OrderDetailsMaps.ToList().Exists(e => e.OrderDetailId == item.OrderDetailId);

        //        if (!isMatched)
        //        {
        //            OrderDetailsMap mapObject = new OrderDetailsMap();
        //            mapObject = await _orderRepository.GetOrderDetailAsync(item.OrderDetailId);
        //            mapObject.IsActive = false;
        //        }
        //    }
        //    await _orderRepository.SaveChangesAsync();

        //    //Add new records
        //    foreach (var item in orderModel.OrderDetailsMaps)
        //    {
        //        var isMatched = saleOrder.OrderDetailsMaps.ToList().Exists(e => e.OrderDetailId == item.OrderDetailId);
        //        OrderDetailsMap mapObject = new OrderDetailsMap();
        //        if (isMatched)
        //        {
        //            mapObject = await _orderRepository.GetOrderDetailAsync(item.OrderDetailId);
        //            mapObject.Order = saleOrder;
        //            mapObject.OrderId = saleOrder.OrderId;
        //            mapObject.ProductId = item.ProductId;
        //            mapObject.SalePrice = item.SalePrice;
        //            mapObject.Qty = item.Qty;
        //            mapObject.ProductColourId = item.ProductColourId;
        //            mapObject.ProductSizeId = item.ProductSizeId;
        //        }
        //        else
        //        {
        //            mapObject.Order = saleOrder;
        //            mapObject.OrderId = saleOrder.OrderId;
        //            mapObject.ProductId = item.ProductId;
        //            mapObject.SalePrice = item.SalePrice;
        //            mapObject.Qty = item.Qty;
        //            mapObject.ProductColourId = item.ProductColourId;
        //            mapObject.ProductSizeId = item.ProductSizeId;
        //            _orderRepository.Add(mapObject);
        //        }
        //    }
        //    await _orderRepository.SaveChangesAsync();
        //    var historyRecord = CreateHistoryRecord(orderModel.OrderId, "Edited", orderModel.PaymentMethod);
        //    await CreateOrderHistoryAsync(historyRecord);

        //    List<string> resultList = new List<string>();
        //    resultList.Add("Updated successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<OrderDetailsMap>> GetOrderDetailsAsync(int orderId)
        //{
        //    return await _orderRepository.GetOrderDetailsAsync(orderId);
        //}

        //public async Task<OrderDetailsMap> GetOrderDetailAsync(int orderDetailId)
        //{
        //    return await _orderRepository.GetOrderDetailAsync(orderDetailId);
        //}

        //public async Task<IEnumerable<OrderDetailsMap>> GetPagedOrderDetailsAsync(int orderId)
        //{
        //    return await _orderRepository.GetPagedOrderDetailsAsync(orderId);
        //}

        //#endregion

        //#region OrderHistoryMap

        //public async Task<IEnumerable<OrderHistoryMap>> GetOrderHistoryAsync(int orderId)
        //{
        //    return await _orderRepository.GetOrderHistoryAsync(orderId);
        //}

        //public async Task<OrderHistoryMap> GetOrderHistoryDetailsAsync(int orderHistoryId)
        //{
        //    return await _orderRepository.GetOrderHistoryDetailsAsync(orderHistoryId);
        //}

        //public async Task<IEnumerable<OrderHistoryMap>> GetPagedOrderHistoryDetailsAsync(int orderId)
        //{
        //    return await _orderRepository.GetPagedOrderHistoryDetailsAsync(orderId);
        //}

        //public async Task<IEnumerable<string>> CreateOrderHistoryAsync(OrderHistoryMap request)
        //{
        //    _orderRepository.Add(request);
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Created successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<string>> UpdateOrderHistoryAsync(OrderHistoryMap request)
        //{
        //    var OrderPaymentMap = await _orderRepository.GetOrderHistoryDetailsAsync(request.OrderHistoryId);
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Updated successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<string>> DeleteOrderHistoryAsync(int orderHistoryId)
        //{
        //    var orderHistoryDetail = await _orderRepository.GetOrderHistoryDetailsAsync(orderHistoryId);
        //    orderHistoryDetail.IsActive = false;
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Deleted successfully");
        //    return resultList;
        //}
        //#endregion

        //#region OrderPaymentMap

        //public async Task<IEnumerable<OrderPaymentMap>> GetOrderPaymentsAsync(int orderId)
        //{
        //    return await _orderRepository.GetOrderPaymentsAsync(orderId);
        //}

        //public async Task<OrderPaymentMap> GetOrderPaymentDetailsAsync(int orderPaymentDetailId)
        //{
        //    return await _orderRepository.GetOrderPaymentDetailsAsync(orderPaymentDetailId);
        //}

        //public async Task<IEnumerable<OrderPaymentMap>> GetPagedOrderPaymentsAsync(int orderId)
        //{
        //    return await _orderRepository.GetPagedOrderPaymentsAsync(orderId);
        //}


        //public async Task<IEnumerable<string>> CreateOrderPaymentAsync(OrderPaymentMap request)
        //{
        //    _orderRepository.Add(request);
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Created successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<string>> UpdateOrderPaymentAsync(OrderPaymentMap request)
        //{
        //    var OrderPaymentMap = await _orderRepository.GetOrderPaymentDetailsAsync(request.OrderPaymentId);
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Updated successfully");
        //    return resultList;
        //}

        //public async Task<IEnumerable<string>> DeleteOrderPaymentAsync(int orderPaymentId)
        //{
        //    var orderHistoryDetail = await _orderRepository.GetOrderPaymentDetailsAsync(orderPaymentId);
        //    orderHistoryDetail.IsActive = false;
        //    await _orderRepository.SaveChangesAsync();
        //    List<string> resultList = new List<string>();
        //    resultList.Add("Deleted successfully");
        //    return resultList;
        //}

        //#endregion


        #region Private Methods

        private async Task<int> CreateOrder(OrderDetailsModel request)
        {
            var orderModel = new Order()
            {
                UserId = request.UserId,
                ShopId = request.ShopId,
                ItemTotal = request.ItemTotal,
                NetAmount = request.ItemTotal,
                VatAmount = request.VatAmount,
                DiscountAmount = request.DiscountAmount,
                DeliveryCharges = request.DeliveryCharges,
                TotalWithOutVATAmount = request.TotalWithOutVATAmount,
                TotalWithVATAmount = request.TotalWithVATAmount,
                VatPercentage = request.VatPercentage,
                DiscountPercentage = request.DiscountPercentage,
                CouponCode = request.CouponCode,
                OrderPaymentTypeId = request.PaymentMethodId,
                OrderStatusTypeId = 1,//Pending order
                OrderDeliveryTypeId = request.ShippingModeId,
                TrackingNumber = request.TrackingNumber,
                ShippingAddress = request.ShippingAddress,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
                IsRead = 0
            };
            _orderRepository.Add(orderModel);
            await _orderRepository.SaveChangesAsync();
            return orderModel.OrderId;
        }

        private async Task UpdateOrder(OrderDetailsModel request)
        {
            var orderModel = await _orderRepository.GetByIdAsync(request.OrderId ?? 0);
            orderModel.ItemTotal = request.ItemTotal;
            orderModel.NetAmount = request.ItemTotal;
            orderModel.VatAmount = request.VatAmount;
            orderModel.DiscountAmount = request.DiscountAmount;
            orderModel.DeliveryCharges = request.DeliveryCharges;
            orderModel.TotalWithOutVATAmount = request.TotalWithOutVATAmount;
            orderModel.TotalWithVATAmount = request.TotalWithVATAmount;
            orderModel.VatPercentage = request.VatPercentage;
            orderModel.DiscountPercentage = request.DiscountPercentage;
            orderModel.CouponCode = request.CouponCode;
            orderModel.ModifiedDate = DateTime.Now;
            orderModel.ModifiedBy = 1;
            await _orderRepository.SaveChangesAsync();
        }

        private async Task CreateHistoryRecord(OrderDetailsModel request, string? comments)
        {
            OrderHistory OrderHistoryMap = new OrderHistory();
            OrderHistoryMap.OrderId = request.OrderId ?? 0;
            OrderHistoryMap.OrderStatusTypeId = request.OrderStatusId;
            OrderHistoryMap.OrderPaymentTypeId = request.PaymentMethodId;
            OrderHistoryMap.OrderDeliveryTypeId = request.ShippingModeId;
            OrderHistoryMap.TrackingNumber = request.TrackingNumber;
            OrderHistoryMap.Comments = comments;
            OrderHistoryMap.IsActive = true;
            OrderHistoryMap.CreatedDate = DateTime.Now;
            OrderHistoryMap.CreatedBy = 1;

            _orderRepository.Add(OrderHistoryMap);
            await _orderRepository.SaveChangesAsync();
        }

    }

    #endregion
}

