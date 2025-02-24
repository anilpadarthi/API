using AutoMapper;
using Azure;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Repository.Repositories;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICommissionStatementRepository _commissionRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICommissionStatementRepository commissionRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _commissionRepository = commissionRepository;
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
                foreach (var product in result.Items.ToList())
                {
                    product.ProductImage = FileUtility.GetImagePath(FolderUtility.product, product.ProductImage);
                    product.ProductPrices = (await _productRepository.GetProductPricesAsync(product.ProductId ?? 0)).ToList();
                }

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


        public async Task<CommonResponse> CreateOrderPaymentAsync(OrderPaymentDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                bool isRedemed = false;
                if (request.PaymentMode == "CommissionCheque")
                {
                    var commisionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(Convert.ToInt32(request.ReferenceNumber));
                    if (commisionHistoryDetails != null)
                    {
                        isRedemed = commisionHistoryDetails.IsRedemed;
                    }
                }

                if (isRedemed)
                {
                    response = Utility.CreateResponse("Already redemed.", HttpStatusCode.Conflict);
                }
                else
                {
                    var obj = _mapper.Map<OrderPayment>(request);
                    obj.PaymentDate = DateTime.Now;
                    obj.CreatedDate = DateTime.Now;
                    obj.CollectedStatus = "PPA";
                    obj.Status = (short)EnumStatus.Active;
                    if (request.ReferenceImage != null)
                    {
                        obj.ReferenceImage = FileUtility.uploadImage(request.ReferenceImage, FolderUtility.paymentProofs);
                    }
                    _orderRepository.Add(obj);

                    if (request.PaymentMode == "CommissionCheque")
                    {
                        var commisionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(Convert.ToInt32(request.ReferenceNumber));
                        if (commisionHistoryDetails != null)
                        {
                            commisionHistoryDetails.IsRedemed = true;
                        }
                    }
                    await _orderRepository.SaveChangesAsync();
                    response = Utility.CreateResponse("Saved successfully", HttpStatusCode.Created);
                }

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateOrderPaymentAsync(int orderPaymentId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var orderPaymentData = await _orderRepository.GetOrderPaymentDetailsAsync(orderPaymentId);
                orderPaymentData.CollectedStatus = "PPS";
                orderPaymentData.ModifiedDate = DateTime.Now;
                await _orderRepository.SaveChangesAsync();
                response = Utility.CreateResponse("Saved successfully", HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteOrderPaymentAsync(int orderPaymentId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var orderPaymentData = await _orderRepository.GetOrderPaymentDetailsAsync(orderPaymentId);
                orderPaymentData.Status = (short)EnumStatus.Deleted;
                orderPaymentData.ModifiedDate = DateTime.Now;
                await _orderRepository.SaveChangesAsync();
                response = Utility.CreateResponse(orderPaymentData, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }



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
            if (request.requestType == "MonthlyShopCommission")
            {
                var commissionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(request.referenceNumber ?? 0);
                if (commissionHistoryDetails != null)
                {
                    commissionHistoryDetails.IsRedemed = true;
                    commissionHistoryDetails.IsOptedForCheque = false;
                    await _commissionRepository.SaveChangesAsync();
                }
            }
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

