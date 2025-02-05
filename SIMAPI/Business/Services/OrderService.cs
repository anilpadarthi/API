using AutoMapper;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
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

        public async Task<CommonResponse> CreateAsync(OrderDetailDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                int orderId = 0;
                if (request != null)
                {
                    orderId = await CreateOrder(request);
                }

                foreach (var item in request.items)
                {
                    OrderDetail mapObject = new OrderDetail()
                    {
                        OrderId = orderId,
                        ProductId = item.ProductId,
                        SalePrice = item.SalePrice,
                        Qty = item.Qty,
                        //ProductColourId = item.ProductColourId,
                        //ProductSizeId = item.ProductSizeId,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = request.loggedInUserId.Value
                    };
                    _orderRepository.Add(mapObject);
                }
                await _orderRepository.SaveChangesAsync();
                request.orderId = orderId;
                await CreateHistoryRecord(request, "Created");
                response = Utility.CreateResponse("Order placed successfully", HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(OrderDetailDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                int orderId = request.orderId ?? 0;
                if (request != null && orderId > 0)
                {
                    await UpdateOrder(request);
                    var savedItems = (await _orderRepository.GetItemsAsync(orderId)).ToList();

                    //update existing items as inactive if not found in the requested items
                    foreach (var item in savedItems)
                    {
                        var matchedItem = request.items.Where(e => e.ProductId == item.ProductId).FirstOrDefault();
                        if (matchedItem != null)
                        {
                            item.Qty = matchedItem.Qty;
                            item.SalePrice = matchedItem.SalePrice;
                            item.ProductSizeId = matchedItem.ProductSizeId;
                            item.ProductColourId = matchedItem.ProductColourId;
                            item.ModifiedDate = DateTime.Now;
                            item.ModifiedBy = 1;
                        }
                        else
                        {
                            item.IsActive = false;
                        }
                    }
                    await _orderRepository.SaveChangesAsync();

                    foreach (var item in request.items)
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
                                CreatedBy = request.loggedInUserId.Value
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


        public async Task<CommonResponse> UpdateOrderDetailsAsync(OrderStatusModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var order = await _orderRepository.GetByIdAsync(request.OrderId);
                order.OrderStatusTypeId = request.OrderStatusId;
                order.OrderPaymentTypeId = request.PaymentMethodId;
                order.OrderDeliveryTypeId = request.ShippingModeId;
                order.TrackingNumber = request.TrackingNumber;
                order.ModifiedBy = request.loggedInUserId.Value;
                order.ModifiedDate = DateTime.Now;
                await _orderRepository.SaveChangesAsync();

                OrderDetailDto request1 = new OrderDetailDto();
                request1.orderId = request.OrderId;
                request1.orderStatusId = request.OrderStatusId;
                request1.paymentMethodId = request.PaymentMethodId;
                request1.shippingModeId = request.ShippingModeId;
                request1.trackingNumber = request.TrackingNumber;
                request1.loggedInUserId = request.loggedInUserId;
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

        public async Task<CommonResponse> GetShoppingPageDetailsAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.GetShoppingPageDetailsAsync();
                if (result != null)
                {
                    result.Categories?.ToList().ForEach(category =>
                    {
                        category.Image = FileUtility.GetImagePath(FolderUtility.category, category.Image);

                        category.SubCategories?.ToList().ForEach(subCategory =>
                        {
                            subCategory.Image = FileUtility.GetImagePath(FolderUtility.subCategory, subCategory.Image);
                        });
                    });
                    result.Products?.ToList().ForEach(product =>
                    {
                        product.ProductImage = FileUtility.GetImagePath(FolderUtility.product, product.ProductImage);
                    });
                }

                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetPagedOrderListAsync(GetPagedOrderListDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _orderRepository.GetOrdersByPagingAsync(request);
                pageResult.TotalRecords = await _orderRepository.GetTotalOrdersCountAsync(request);
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

        public async Task<CommonResponse> GetOrderPaymentHistoryAsync(int orderId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.GetOrderPaymentHistoryAsync(orderId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;

        }

        public async Task<CommonResponse> DownloadOrderListAsync(GetPagedOrderListDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var orderList = await _orderRepository.GetOrdersByPagingAsync(request);
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

        private async Task<int> CreateOrder(OrderDetailDto request)
        {
            var orderModel = new Order()
            {
                UserId = request.loggedInUserId,
                PlacedBy = request.placedBy ?? 0,
                ShopId = request.shopId ?? 0,
                ItemTotal = request.itemTotal ?? 0,
                NetAmount = request.itemTotal ?? 0,
                VatAmount = request.vatAmount ?? 0,
                DiscountAmount = request.discountAmount ?? 0,
                DeliveryCharges = request.deliveryCharges ?? 0,
                TotalWithOutVATAmount = request.totalWithOutVATAmount ?? 0,
                TotalWithVATAmount = request.totalWithVATAmount ?? 0,
                VatPercentage = request.vatPercentage ?? 0,
                DiscountPercentage = request.discountPercentage ?? 0,
                CouponCode = request.couponCode,
                OrderPaymentTypeId = request.paymentMethodId,
                OrderStatusTypeId = (int)EnumOrderStatus.Pendig,
                OrderDeliveryTypeId = request.shippingModeId,
                TrackingNumber = request.trackingNumber,
                ShippingAddress = request.shippingAddress,
                RequestType = request.requestType,
                CreatedDate = DateTime.Now,
                CreatedBy = request.loggedInUserId.Value,
                IsRead = 0
            };
            _orderRepository.Add(orderModel);
            await _orderRepository.SaveChangesAsync();
            return orderModel.OrderId;
        }

        private async Task UpdateOrder(OrderDetailDto request)
        {
            var orderModel = await _orderRepository.GetByIdAsync(request.orderId ?? 0);
            orderModel.ItemTotal = request.itemTotal;
            orderModel.NetAmount = request.itemTotal;
            orderModel.VatAmount = request.vatAmount;
            orderModel.DiscountAmount = request.discountAmount;
            orderModel.DeliveryCharges = request.deliveryCharges;
            orderModel.TotalWithOutVATAmount = request.totalWithOutVATAmount;
            orderModel.TotalWithVATAmount = request.totalWithVATAmount;
            orderModel.VatPercentage = request.vatPercentage;
            orderModel.DiscountPercentage = request.discountPercentage;
            orderModel.CouponCode = request.couponCode;
            orderModel.ModifiedDate = DateTime.Now;
            orderModel.ModifiedBy = request.loggedInUserId.Value;
            await _orderRepository.SaveChangesAsync();
        }

        private async Task CreateHistoryRecord(OrderDetailDto request, string? comments)
        {
            OrderHistory OrderHistoryMap = new OrderHistory();
            OrderHistoryMap.OrderId = request.orderId ?? 0;
            OrderHistoryMap.OrderStatusTypeId = request.orderStatusId;
            OrderHistoryMap.OrderPaymentTypeId = request.paymentMethodId;
            OrderHistoryMap.OrderDeliveryTypeId = request.shippingModeId;
            OrderHistoryMap.TrackingNumber = request.trackingNumber;
            OrderHistoryMap.Comments = comments;
            OrderHistoryMap.IsActive = true;
            OrderHistoryMap.CreatedDate = DateTime.Now;
            OrderHistoryMap.CreatedBy = request.loggedInUserId.Value;

            _orderRepository.Add(OrderHistoryMap);
            await _orderRepository.SaveChangesAsync();
        }

    }

    #endregion
}

