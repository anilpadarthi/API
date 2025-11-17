using AutoMapper;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Helper.PDF;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data;
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
        private readonly IProductRepository _productRepository;
        private readonly ICommissionStatementRepository _commissionRepository;
        private readonly IMapper _mapper;
        private readonly SIMDBContext _context;

        public OrderService(IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICommissionStatementRepository commissionRepository, IMapper mapper,
            SIMDBContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _commissionRepository = commissionRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CommonResponse> CreateAsync(OrderDetailDto request)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
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
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _orderRepository);
                }
                var invoiceDetails = await _orderRepository.GetOrderDetailsForInvoiceByIdAsync(request.orderId.Value);
                CommunicationHelper.SendOrderConfirmationEmail(invoiceDetails);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(OrderDetailDto request)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
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

                        await CreateHistoryRecord(request, "Updated Order Details");
                        await _orderRepository.SaveChangesAsync();
                        response = Utility.CreateResponse("Order updated successfully", HttpStatusCode.OK);
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _orderRepository);
                }
                return response;
            }
        }


        public async Task<CommonResponse> UpdateOrderDetailsAsync(OrderStatusModel request)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var order = await _orderRepository.GetByIdAsync(request.OrderId);
                    order.OrderStatusTypeId = request.OrderStatusId;
                    order.OrderPaymentTypeId = request.PaymentMethodId;
                    order.OrderDeliveryTypeId = request.ShippingModeId;
                    order.TrackingNumber = request.TrackingNumber;
                    order.ModifiedBy = request.loggedInUserId.Value;
                    order.ModifiedDate = DateTime.Now;


                    OrderDetailDto request1 = new OrderDetailDto();
                    request1.orderId = request.OrderId;
                    request1.orderStatusId = request.OrderStatusId;
                    request1.paymentMethodId = request.PaymentMethodId;
                    request1.shippingModeId = request.ShippingModeId;
                    request1.trackingNumber = request.TrackingNumber;
                    request1.loggedInUserId = request.loggedInUserId;
                    await CreateHistoryRecord(request1, "Updated_" + request.ShippingModeId + "_" + request.TrackingNumber);

                    //order cancelled
                    if (request.OrderStatusId == (int)EnumOrderStatus.Cancelled)
                    {
                        var walletHistory = await _orderRepository.GetShopWalletHistoryByReferenceNumber("O-" + Convert.ToString(order.OrderId), "Debit");
                        if (walletHistory != null && walletHistory.Any())
                        {
                            foreach (var item in walletHistory.ToList())
                            {
                                item.IsActive = false;
                                item.CancelledDate = DateTime.Now;
                                item.CancelledReason = "Order cancelled";
                            }
                            await _orderRepository.SaveChangesAsync();
                        }

                        var commissionHistoryList = await _commissionRepository.GetCommissionHistoryListAsync("O-" + Convert.ToString(order.OrderId));
                        if (commissionHistoryList != null && commissionHistoryList.Any())
                        {
                            foreach (var item in commissionHistoryList.ToList())
                            {
                                item.IsRedemed = false;
                                item.OptInType = "";
                            }
                            await _commissionRepository.SaveChangesAsync();
                        }

                        //await DeleteOrderPaymentAsync();
                    }

                    response = Utility.CreateResponse("Updated status successfully", HttpStatusCode.OK);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _orderRepository);
                }
                return response;
            }
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
                response = response.HandleException(ex, _orderRepository);
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
                    //result.Products?.ToList().ForEach(product =>
                    //{
                    //    product.ProductImage = FileUtility.GetImagePath(FolderUtility.product, product.ProductImage);
                    //});
                }

                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetProductListAsync(int categoryId, int subCategoryId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.GetProductListAsync(categoryId, subCategoryId);
                if (result != null)
                {
                    result.ToList().ForEach(product =>
                    {
                        product.ProductImage = FileUtility.GetImagePath(FolderUtility.product, product.ProductImage);
                    });
                }
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetNewArrivalsAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.GetNewArrivalsAsync();
                if (result != null)
                {
                    result.ToList().ForEach(product =>
                    {
                        product.ProductImage = FileUtility.GetImagePath(FolderUtility.product, product.ProductImage);
                    });
                }
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
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
                response = response.HandleException(ex, _orderRepository);
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
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }

        public async Task<CommonResponse> HideOrderAsync(int orderId, bool isHide)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var orderInfo = await _orderRepository.GetByIdAsync(orderId);
                orderInfo.IsHide = isHide;
                await _orderRepository.SaveChangesAsync();
                response = Utility.CreateResponse("Successfully hidden", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
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
                response = response.HandleException(ex, _orderRepository);
            }
            return response;

        }

        public async Task<CommonResponse> DownloadOrderListAsync(GetPagedOrderListDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var orderList = await _orderRepository.DownloadOrderListAsync(request);
                response = Utility.CreateResponse(orderList, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
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
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }


        public async Task<CommonResponse> CreateOrderPaymentAsync(OrderPaymentDto request)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    bool isRedemed = false;
                    var optInType = "";
                    if (request.PaymentMode == "CommissionCheque")
                    {
                        var commisionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(Convert.ToInt32(request.ReferenceNumber));
                        if (commisionHistoryDetails != null)
                        {
                            isRedemed = commisionHistoryDetails.IsRedemed;
                            optInType = commisionHistoryDetails.OptInType;
                        }
                    }

                    if (optInType == "Accessories")
                    {
                        response = Utility.CreateResponse("Already redemed.", HttpStatusCode.Conflict);
                    }
                    else
                    {
                        var obj = _mapper.Map<OrderPayment>(request);
                        obj.PaymentDate = DateTime.Now;
                        obj.CreatedDate = DateTime.Now;
                        obj.OrderId = request.OrderId;
                        obj.CollectedStatus = request.PaymentMode == "Cash" || request.PaymentMode == "BankTransfer" ? EnumOrderStatus.PPA.ToString() : EnumOrderStatus.PPS.ToString();
                        obj.PaymentMode = request.PaymentMode;
                        obj.UserId = request.UserId;
                        obj.Status = (short)EnumStatus.Active;
                        if (request.ReferenceImage != null)
                        {
                            obj.ReferenceImage = FileUtility.uploadImage(request.ReferenceImage, FolderUtility.paymentProofs);
                        }
                        _orderRepository.Add(obj);
                        await _orderRepository.SaveChangesAsync();
                        if (request.PaymentMode == "CommissionCheque")
                        {
                            var commisionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(Convert.ToInt32(request.ReferenceNumber));
                            if (commisionHistoryDetails != null)
                            {
                                commisionHistoryDetails.IsRedemed = true;
                                commisionHistoryDetails.OptInType = "Accessories";

                                await AddShopWalletHistoryRecord(commisionHistoryDetails.CommissionAmount ?? 0, request);
                            }
                            await _commissionRepository.SaveChangesAsync();
                        }
                        else if (request.PaymentMode == "Commission")
                        {
                            await AddShopWalletHistoryRecord(request.Amount, request);
                        }
                        else if (request.PaymentMode == "Bonus")
                        {
                            await AddShopWalletHistoryRecord(request.Amount, request);
                        }
                        else if (request.PaymentMode == "InstantBonus")
                        {
                            await AddShopWalletHistoryRecord(request.Amount, request);
                        }
                        else
                        {
                            PaymentReceiptModel model = new PaymentReceiptModel()
                            {
                                ReceiptNo = "R-" + obj.OrderPaymentId.ToString(),
                                CustomerName = "",
                                CustomerPhone = "",
                                PaymentDate = DateTime.Now,
                                AmountPaid = request.Amount,
                                PaymentMethod = obj.PaymentMode,
                                Remarks = request.Comments,
                                ShopEmail = "",
                                OrderId = request.OrderId
                            };
                            CommunicationHelper.SendPaymentReceiptEmail(model);
                        }
                        response = Utility.CreateResponse("Saved successfully", HttpStatusCode.Created);
                    }
                    await transaction.CommitAsync();

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _orderRepository);
                }
                return response;
            }
        }

        public async Task<CommonResponse> UpdateOrderPaymentAsync(int orderPaymentId, int userRoleId)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderPaymentData = await _orderRepository.GetOrderPaymentDetailsAsync(orderPaymentId);
                    orderPaymentData.CollectedStatus = (userRoleId == (int)EnumUserRole.Manager) ? EnumOrderStatus.PPM.ToString() : EnumOrderStatus.PPS.ToString();
                    orderPaymentData.ModifiedDate = DateTime.Now;
                    await _orderRepository.SaveChangesAsync();
                    await _orderRepository.VerifyAndUpdatePaidStatus(orderPaymentData.OrderId);
                    response = Utility.CreateResponse("Saved successfully", HttpStatusCode.OK);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _orderRepository);
                }
                return response;
            }
        }

        public async Task<CommonResponse> DeleteOrderPaymentAsync(int orderPaymentId)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var orderPaymentData = await _orderRepository.GetOrderPaymentDetailsAsync(orderPaymentId);
                    orderPaymentData.Status = (short)EnumStatus.Deleted;
                    orderPaymentData.ModifiedDate = DateTime.Now;
                    await _orderRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(orderPaymentData, HttpStatusCode.OK);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _orderRepository);
                }
                return response;
            }
        }



        public async Task<CommonResponse> GeneratePdfInvoiceAsync(int orderId, bool isVAT)
        {
            CommonResponse response = new CommonResponse();
            byte[] result = null;
            try
            {
                PDFInvoice pdfInvoice = new PDFInvoice();
                var invoiceDetailModel = await _orderRepository.GetOrderDetailsForInvoiceByIdAsync(orderId);
                result = pdfInvoice.GenerateInvoice(invoiceDetailModel, isVAT);

                if (isVAT)
                {
                    var orderInfo = await _orderRepository.GetByIdAsync(orderId);
                    orderInfo.IsVat = 1;
                    await _orderRepository.SaveChangesAsync();
                }
                if (result != null && result.Length > 0)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("invoice does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }

        public async Task<CommonResponse> SendInvoiceAsync(int orderId, bool isVAT)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (isVAT)
                {
                    var orderInfo = await _orderRepository.GetByIdAsync(orderId);
                    orderInfo.IsVat = 1;
                    await _orderRepository.SaveChangesAsync();
                }
                var invoiceDetails = await _orderRepository.GetOrderDetailsForInvoiceByIdAsync(orderId);
                CommunicationHelper.SendInvoiceEmail(invoiceDetails,isVAT);
                response = Utility.CreateResponse(true, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }


        public async Task<CommonResponse> LoadOutstandingMetricsAsync(string filterType, int filterId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _orderRepository.LoadOutstandingMetricsAsync(filterType, filterId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _orderRepository);
            }
            return response;
        }


        #region Private Methods

        private async Task<int> CreateOrder(OrderDetailDto request)
        {
            var unpaidCount = request.requestType == "COD" ? await _orderRepository.GetUnpaidOrdersCount(request.shopId ?? 0) : 0;
            var orderModel = new OrderInfo()
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
                OrderStatusTypeId = unpaidCount >= 2 ? (int)EnumOrderStatus.Hold : (int)EnumOrderStatus.Pendig,
                OrderDeliveryTypeId = request.shippingModeId,
                TrackingNumber = request.trackingNumber,
                ShippingAddress = request.shippingAddress,
                RequestType = request.requestType,
                CreatedDate = DateTime.Now,
                CreatedBy = request.loggedInUserId.Value,
                IsRead = 0,
                IsVat = request.isVat,
                WalletAmount = request.walletAmount
            };
            _orderRepository.Add(orderModel);
            await _orderRepository.SaveChangesAsync();
            if (request.requestType == "MC")
            {
                var commissionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(request.referenceNumber ?? 0);
                if (commissionHistoryDetails != null)
                {
                    commissionHistoryDetails.IsRedemed = true;
                    commissionHistoryDetails.OptInType = "Accessories";
                    commissionHistoryDetails.ReferenceNumber = "O-" + Convert.ToString(orderModel.OrderId);
                    await _commissionRepository.SaveChangesAsync();

                    orderModel.WalletAmount = commissionHistoryDetails.CommissionAmount ?? 0;
                    await CreateOrderPaymentForCommission(orderModel, request.referenceNumber.Value, "Monthly Commission");
                }
            }
            else if (request.requestType == "COD" && request.walletAmount > 0)
            {
                var commissionHistoryDetails = await _commissionRepository.GetCommissionHistoryDetailsAsync(request.referenceNumber ?? 0);
                if (commissionHistoryDetails != null && commissionHistoryDetails.IsRedemed == false)
                {
                    commissionHistoryDetails.IsRedemed = true;
                    commissionHistoryDetails.OptInType = "Accessories";
                    commissionHistoryDetails.ReferenceNumber = "O-" + Convert.ToString(orderModel.OrderId);
                    await _commissionRepository.SaveChangesAsync();
                }

                //Create wallet record
                ShopWalletHistory shopWalletHistory = new ShopWalletHistory();
                shopWalletHistory.Amount = orderModel.WalletAmount ?? 0;
                shopWalletHistory.TransactionType = "Debit";
                shopWalletHistory.ReferenceNumber = "O-" + Convert.ToString(orderModel.OrderId);
                shopWalletHistory.ShopId = orderModel.ShopId.Value;
                shopWalletHistory.UserId = orderModel.UserId.Value;
                shopWalletHistory.WalletType = "Commission";
                shopWalletHistory.TransactionDate = DateTime.Now;
                shopWalletHistory.IsActive = true;
                shopWalletHistory.Comments = "Accessories order placed -" + orderModel.OrderId;
                _orderRepository.Add(shopWalletHistory);
                await _orderRepository.SaveChangesAsync();
                await CreateOrderPaymentForCommission(orderModel, request.referenceNumber.Value, "Commission Wallet");

            }
            else if (request.requestType == "B")
            {
                //Create wallet record
                ShopWalletHistory shopWalletHistory = new ShopWalletHistory();
                shopWalletHistory.Amount = request.totalWithVATAmount ?? 0;
                shopWalletHistory.TransactionType = "Debit";
                shopWalletHistory.ReferenceNumber = "O-" + Convert.ToString(orderModel.OrderId);
                shopWalletHistory.ShopId = orderModel.ShopId.Value;
                shopWalletHistory.UserId = orderModel.UserId.Value;
                shopWalletHistory.WalletType = "Bonus";
                shopWalletHistory.TransactionDate = DateTime.Now;
                shopWalletHistory.IsActive = true;
                shopWalletHistory.Comments = "Accessories order placed -" + orderModel.OrderId;
                _orderRepository.Add(shopWalletHistory);
                await _orderRepository.SaveChangesAsync();
                orderModel.WalletAmount = request.totalWithVATAmount ?? 0;
                await CreateOrderPaymentForCommission(orderModel, orderModel.OrderId, "Bonus Wallet");

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

            if (orderModel.OrderPaymentTypeId == (int)EnumOrderPaymentMethod.Bonus)
            {
                var walletHistory = await _orderRepository.GetShopWalletHistoryByReferenceNumber(Convert.ToString(orderModel.OrderId), "Debit");
                if (walletHistory != null && walletHistory.Any())
                {
                    foreach (var item in walletHistory.ToList())
                    {
                        item.Amount = orderModel.TotalWithVATAmount.Value;
                    }
                    await _orderRepository.SaveChangesAsync();
                }
            }

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


        private async Task CreateOrderPaymentForCommission(OrderInfo orderModel, int referenceNumber, string walletType)
        {
            //Create payment record
            OrderPayment orderPayment = new OrderPayment();
            orderPayment.PaymentDate = DateTime.Now;
            orderPayment.CreatedDate = DateTime.Now;
            orderPayment.OrderId = orderModel.OrderId;
            orderPayment.UserId = orderModel.UserId;
            orderPayment.ShopId = orderModel.ShopId;
            orderPayment.Amount = orderModel.WalletAmount ?? 0;
            orderPayment.CollectedStatus = EnumOrderStatus.PPS.ToString();
            orderPayment.PaymentMode = walletType;
            orderPayment.Comments = "Debited using " + walletType;
            orderPayment.Status = (short)EnumStatus.Active;
            orderPayment.ReferenceNumber = Convert.ToString(referenceNumber);
            _orderRepository.Add(orderPayment);
            await _orderRepository.SaveChangesAsync();
        }

        private async Task AddShopWalletHistoryRecord(decimal commissionAmount, OrderPaymentDto request)
        {
            try
            {
                ShopWalletHistory shopWalletHistory = new ShopWalletHistory();
                shopWalletHistory.Amount = commissionAmount;
                shopWalletHistory.TransactionType = "Debit";
                shopWalletHistory.ReferenceNumber = "O-" + Convert.ToString(request.OrderId);
                shopWalletHistory.ShopId = request.ShopId;
                shopWalletHistory.UserId = request.UserId.Value;
                shopWalletHistory.WalletType = request.PaymentMode;
                shopWalletHistory.TransactionDate = DateTime.Now;
                shopWalletHistory.IsActive = true;
                shopWalletHistory.Comments = "Payment for order - " + request.OrderId;
                _orderRepository.Add(shopWalletHistory);
                await _orderRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }
    }

    #endregion
}

