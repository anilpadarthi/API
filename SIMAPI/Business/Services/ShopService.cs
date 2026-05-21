using AutoMapper;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OnField;
using SIMAPI.Repository.Interfaces;
using System.Net;


namespace SIMAPI.Business.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;
        private readonly SIMDBContext _context;
        public ShopService(IShopRepository ShopRepository, IMapper mapper, SIMDBContext context)
        {
            _shopRepository = ShopRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<CommonResponse> CreateAsync(ShopDto request)
        {
            CommonResponse response = new CommonResponse();
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

                var shopDbo = await _shopRepository.GetShopByNameAsync(request.ShopName, request.PostCode);
                if (shopDbo != null)
                {
                    response = Utility.CreateResponse("Shop is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    shopDbo = _mapper.Map<Shop>(request);
                    shopDbo.Status = (short)EnumStatus.Active;
                    shopDbo.CreatedDate = DateTime.Now;
                    shopDbo.UpdatedDate = DateTime.Now;
                    shopDbo.Password = CommunicationHelper.GeneratePassword(8);
                    if (request.ImageFile != null)
                    {
                        shopDbo.Image = await FileUtility.UploadImageAsync(request.ImageFile, FolderUtility.shop);
                    }
                    shopDbo.OldShopId = await _shopRepository.GetNextOldShopIdAsync() + 1;
                    _shopRepository.Add(shopDbo);

                    // Save once to get ShopId / persist primary shop record
                    await _shopRepository.SaveChangesAsync();

                    // Add related entities to the same DbContext/transaction but don't call SaveChanges in helpers
                    await CreateShopLog(shopDbo);
                    await CreateShopAgreement(request, shopDbo.ShopId);
                    await CreateShopContacts(request.ShopContacts, shopDbo.ShopId);

                    // Persist all related entities in one SaveChanges call and commit transaction
                    await _shopRepository.SaveChangesAsync();

                    response = Utility.CreateResponse(shopDbo, HttpStatusCode.Created);
                    await transaction.CommitAsync();
                    CommunicationHelper.SendWelcomeEmail(shopDbo.ShopId, shopDbo.ShopName, request.ShopEmail, shopDbo.Password, request.ShopOwnerName);
                    CommunicationHelper.SendRegistrationEmail(shopDbo.ShopId, shopDbo.ShopName, request.ShopEmail, shopDbo.Password, request.ShopOwnerName);
                }

                return response;
            }
            catch
            {   
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<CommonResponse> UpdateAsync(ShopDto request)
        {
            CommonResponse response = new CommonResponse();

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var shopDbo = await _shopRepository.GetShopByNameAsync(request.ShopName, request.PostCode);
                if (shopDbo != null && shopDbo.ShopId != request.ShopId)
                {
                    response = Utility.CreateResponse("Shop name already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    shopDbo = await _shopRepository.GetShopByIdAsync(request.ShopId);                
                    _mapper.Map(request, shopDbo);
                    shopDbo.UpdatedDate = DateTime.Now;
                    if (request.ImageFile != null)
                    {
                        shopDbo.Image = await FileUtility.UploadImageAsync(request.ImageFile, FolderUtility.shop);
                    }

                    // Save the primary shop update first so the entity is tracked and persisted
                    await _shopRepository.SaveChangesAsync();

                    // Add audit/log and related updates in the same transaction but don't persist inside helpers
                    await CreateShopLog(shopDbo);

                    var sAgreement = await _shopRepository.GetShopAgreementAsync(request.ShopId);
                    await UpdateAndCreateShopAgreement(sAgreement, request, shopDbo.ShopId);
                    var sContacts = await _shopRepository.GetShopContactsAsync(request.ShopId);
                    await UpdateOrCreateShopContacts(sContacts, request.ShopContacts, shopDbo.ShopId);

                    // Persist all helper changes in a single save and commit
                    await _shopRepository.SaveChangesAsync();

                    response = Utility.CreateResponse(shopDbo, HttpStatusCode.OK);
                    await transaction.CommitAsync();
                }

                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();

            var shopDBData = await _shopRepository.GetShopByIdAsync(id);
            if (shopDBData != null)
            {
                shopDBData.Status = (short)EnumStatus.Deleted;
                shopDBData.UpdatedDate = DateTime.Now;
                await _shopRepository.SaveChangesAsync();
                await CreateShopLog(shopDBData);
                response = Utility.CreateResponse(shopDBData, HttpStatusCode.OK);
            }
            else
            {
                response = Utility.CreateResponse("Shop name does not exist", HttpStatusCode.NotFound);
            }

            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopDetailsAsync(id);
            if (!string.IsNullOrEmpty(result.shop.Image))
                result.shop.Image = FileUtility.GetImagePath(FolderUtility.shop, result.shop.Image);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name, string postCode)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopByNameAsync(name, postCode);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetAllAsync(int? areaId)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetAllShopsAsync(areaId);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();

            PagedResult pageResult = new PagedResult();
            pageResult.Results = await _shopRepository.GetShopsByPagingAsync(request);
            pageResult.TotalRecords = await _shopRepository.GetTotalShopsCountAsync(request);

            response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> ShopVisitAsync(ShopVisitRequestmodel request)
        {
            CommonResponse response = new CommonResponse();

            if (request.ImageFile != null)
            {
                try
                {
                    // Attempt upload; if it fails we log but still proceed to record the visit
                    request.ReferenceImage = await FileUtility.UploadImageAsync(request.ImageFile, FolderUtility.shopVisit);
                }
                catch (Exception ex)
                {
                    // Log full exception (repository exposes LogError)
                    try
                    {
                        await _shopRepository.LogError(ex, "ShopVisit - UploadImage");
                    }
                    catch
                    {
                        // swallow log failure to avoid masking the original reason
                    }

                    // Clear reference so DB save doesn't point to a missing/partial file
                    request.ReferenceImage = null;
                }
            }

            var result = await _shopRepository.ShopVisitAsync(request);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetShopVisitHistoryAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopVisitHistoryAsync(shopId);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetShopAgreementHistoryAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopAgreementHistoryAsync(shopId);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetShopWalletAmountAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopWalletAmountAsync(shopId);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetShopWalletHistoryAsync(int shopId, string walletType)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopWalletHistoryAsync(shopId, walletType);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetShopAddressDetailsAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopAddressDetailsAsync(shopId);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> SendActivationEmailAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopDetailsAsync(shopId);
            var shopDbo = result.shop;
            var shopContacts = result.shopContacts;
            CommunicationHelper.SendRegistrationEmail(shopDbo.ShopId, shopDbo.ShopName, shopDbo.ShopEmail, shopDbo.Password, "");
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> UpdateAddressAsync(ShippingAddressDetails request)
        {
            CommonResponse response = new CommonResponse();

            var shop = await _shopRepository.GetShopByIdAsync(request.ShopId);
            shop.ShopName = request.ShopName;
            shop.AddressLine1 = request.AddressLine1;
            shop.DeliveryInstructions = request.DeliveryInstructions;
            shop.ShopOwnerName = request.ShopOwnerName;
            shop.ShopEmail = request.ShopEmail;
            shop.ShopPhone = request.ShopPhone;
            shop.PostCode = request.PostCode;
            await _shopRepository.SaveChangesAsync();
            response = Utility.CreateResponse("Saved successfully", HttpStatusCode.OK);

            return response;
        }

        private async Task CreateShopLog(Shop shop)
        {
            var log = _mapper.Map<ShopLog>(shop);
            _shopRepository.Add(log);
            await _shopRepository.SaveChangesAsync();
            // NOTE: Do not call SaveChanges here. Caller manages the transaction and final SaveChanges.
            
        }

        private Task CreateShopAgreement(ShopDto request, int shopId)
        {
            if (request.AgreementFrom.HasValue && request.AgreementTo.HasValue)
            {
                var shopAgreement = new ShopAgreement()
                {
                    AgreementNotes = request.AgreementNotes,
                    FromDate = request.AgreementFrom.Value,
                    ToDate = request.AgreementTo.Value,
                    ShopId = shopId,
                    AgreementBy = request.CreatedBy.HasValue ? request.CreatedBy.Value : 0,
                    Status = (int)EnumStatus.Hold,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                _shopRepository.Add(shopAgreement);
                // Caller will save changes
            }
            return Task.CompletedTask;
        }

        private Task UpdateAndCreateShopAgreement(ShopAgreement savedAgreement, ShopDto request, int shopId)
        {
            // Only proceed if incoming agreement dates exist
            if (request.AgreementFrom.HasValue && request.AgreementTo.HasValue)
            {
                if (savedAgreement != null)
                {
                    // If either date has changed, mark old as inactive and create a new one
                    var fromChanged = savedAgreement.FromDate != request.AgreementFrom.Value;
                    var toChanged = savedAgreement.ToDate != request.AgreementTo.Value;
                    if (fromChanged || toChanged)
                    {
                        savedAgreement.Status = (int)EnumStatus.InActive;
                        savedAgreement.ModifiedDate = DateTime.Now;

                        var shopAgreement = new ShopAgreement()
                        {
                            AgreementNotes = request.AgreementNotes,
                            FromDate = request.AgreementFrom.Value,
                            ToDate = request.AgreementTo.Value,
                            ShopId = shopId,
                            AgreementBy = request.CreatedBy.HasValue ? request.CreatedBy.Value : 0,
                            Status = (int)EnumStatus.Hold,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                        };
                        _shopRepository.Add(shopAgreement);
                    }
                    else
                    {
                        // Dates didn't change; update notes and modified date
                        savedAgreement.AgreementNotes = request.AgreementNotes;
                        savedAgreement.ModifiedDate = DateTime.Now;
                    }
                }
                else
                {
                    // No existing agreement - create new
                    var shopAgreement = new ShopAgreement()
                    {
                        AgreementNotes = request.AgreementNotes,
                        FromDate = request.AgreementFrom.Value,
                        ToDate = request.AgreementTo.Value,
                        ShopId = shopId,
                        AgreementBy = request.CreatedBy.HasValue ? request.CreatedBy.Value : 0,
                        Status = (int)EnumStatus.Hold,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };
                    _shopRepository.Add(shopAgreement);
                }
            }
            else if (savedAgreement != null)
            {
                // If incoming has no agreement but saved one exists, you might want to inactivate it.
                // Decide policy; here we inactivate existing agreement if incoming has no dates.
                savedAgreement.Status = (int)EnumStatus.InActive;
                savedAgreement.ModifiedDate = DateTime.Now;
            }

            // Caller will save changes
            return Task.CompletedTask;
        }


        private Task CreateShopContacts(ShopContactDto[] contacts, int shopId)
        {
            if (contacts != null)
            {
                foreach (var item in contacts)
                {
                    var contact = _mapper.Map<ShopContact>(item);
                    contact.ShopId = shopId;
                    contact.Status = (int)EnumStatus.Active;
                    _shopRepository.Add(contact);
                }
                // Caller will save changes
            }
            return Task.CompletedTask;
        }

        private Task UpdateOrCreateShopContacts(IEnumerable<ShopContact> savedContacts, ShopContactDto[] contacts, int shopId)
        {
            if (savedContacts != null)
            {
                foreach (var savedContact in savedContacts)
                {
                    var matchingContact = contacts != null ? contacts.Where(w => w.ShopContactId == savedContact.ShopContactId).FirstOrDefault() : null;
                    if (matchingContact != null)
                    {
                        // Update existing contact
                        savedContact.ContactName = matchingContact.ContactName;
                        savedContact.ContactEmail = matchingContact.ContactEmail;
                        savedContact.ContactNumber = matchingContact.ContactNumber;
                        savedContact.ContactType = matchingContact.ContactType;
                        savedContact.ModifiedDate = DateTime.Now;
                        // Update other fields as needed
                    }
                    else
                    {
                        // Mark saved contact as inactive or deleted
                        savedContact.Status = (int)EnumStatus.Deleted; // Assuming 0 indicates inactive/deleted
                        savedContact.ModifiedDate = DateTime.Now;
                    }
                }
            }
            if (contacts != null)
            {
                // Process incoming contacts that are new (not found in saved contacts)
                foreach (var item in contacts.Where(c => c.ShopContactId == null || c.ShopContactId == 0))
                {
                    var newContact = _mapper.Map<ShopContact>(item);
                    newContact.ShopId = shopId;
                    newContact.Status = (int)EnumStatus.Active;
                    newContact.CreatedDate = DateTime.Now;
                    _shopRepository.Add(newContact);
                }
            }

            // Caller will save changes
            return Task.CompletedTask;
        }

        public async Task<CommonResponse> GetShopCommissionChequesAsync(int shopId, string mode)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopCommissionChequesAsync(shopId, mode);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetShopCommissionChequeAsync(int sno)
        {
            CommonResponse response = new CommonResponse();

            var result = await _shopRepository.GetShopCommissionChequeAsync(sno);
            response = Utility.CreateResponse(result, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> CreateShopCommissionChequeAsync(CommissionChequeRequestModel request)
        {
            CommonResponse response = new CommonResponse();
            var commissionCheque = await _shopRepository.GetShopCommissionChequeAsync(request.ShopId, request.CommissionDate);
            if (commissionCheque != null)
            {
                response = Utility.CreateResponse("Commission cheque already exists for the month", HttpStatusCode.Conflict);
            }
            else
            {
                // If not exists, create a new one
                commissionCheque = new ShopCommissionCheques
                {
                    ShopId = request.ShopId,
                    ChequeNumber = request.ChequeNumber,
                    CommissionDate = Convert.ToDateTime(request.CommissionDate),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    TotalAmount = request.TotalAmount.ToString(),
                    IsDelete = false
                };
                _shopRepository.Add(commissionCheque);
                await _shopRepository.SaveChangesAsync();
                response = Utility.CreateResponse(commissionCheque, HttpStatusCode.OK);
            }            

            return response;
        }

        public async Task<CommonResponse> UpdateShopCommissionChequeAsync(int sno, string chequeNumber)
        {
            CommonResponse response = new CommonResponse();

            var commissionCheque = await _shopRepository.GetShopCommissionChequeAsync(sno);
            commissionCheque.ChequeNumber = chequeNumber;
            commissionCheque.ModifiedDate = DateTime.Now;
            await _shopRepository.SaveChangesAsync();
            response = Utility.CreateResponse(commissionCheque, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> DeleteShopCommissionChequeAsync(int sno)
        {
            CommonResponse response = new CommonResponse();

            var commissionCheque = await _shopRepository.GetShopCommissionChequeAsync(sno);
            commissionCheque.IsDelete = true;
            commissionCheque.ModifiedDate = DateTime.Now;
            await _shopRepository.SaveChangesAsync();
            response = Utility.CreateResponse(commissionCheque, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GlobalShopSearchAsync(GetLookupRequest request)
        {
            CommonResponse response = new CommonResponse();

            var shopList = await _shopRepository.GlobalShopSearchAsync(request);

            response = Utility.CreateResponse(shopList, HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> GetPendingCommissionTypeChangeRequestsAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            var shopList = await _shopRepository.GetPendingCommissionTypeChangeRequestsAsync(shopId);
            response = Utility.CreateResponse(shopList, HttpStatusCode.OK);
            return response;

        }


        public async Task<CommonResponse> CreateShopCommisioTypeChangeRequestAsync(ShopCommissionRequestDto request)
        {
            CommonResponse response = new CommonResponse();
            ShopCommissionRequest item = new ShopCommissionRequest();
            item.ShopId = request.ShopId;
            item.FromDate = Convert.ToDateTime(request.FromDate);
            item.ToDate = Convert.ToDateTime(request.ToDate);
            item.Status = "Pending";
            item.CreatedBy = request.loggedInUserId.Value;
            item.IsMobileShop = request.isMobileShop;
            item.CreatedDate = DateTime.Now;

            _shopRepository.Add(item);
            await _shopRepository.SaveChangesAsync();
            response = Utility.CreateResponse(request, HttpStatusCode.Created);
            return response;
        }

        public async Task<CommonResponse> UpdateShopCommisioTypeChangeRequestAsync(ShopCommissionRequestDto request)
        {
            CommonResponse response = new CommonResponse();
            var existingRequest = await _shopRepository.GetCommissionTypeChangeRequestAsync(request.ShopCommissionRequestId.Value);
            if (existingRequest != null)
            {
                existingRequest.Status = "Approved";
                existingRequest.ApprovedBy = request.loggedInUserId;
                existingRequest.UpdatedDate = DateTime.Now;
                var existingSHop = await _shopRepository.GetShopByIdAsync(existingRequest.ShopId);
                existingSHop.IsMobileShop = existingRequest.IsMobileShop == 1 ? true : false;
                await _shopRepository.SaveChangesAsync();
                response = Utility.CreateResponse(existingRequest, HttpStatusCode.OK);
            }
            else
            {
                response = Utility.CreateResponse("Request not found", HttpStatusCode.NotFound);
            }
            response = Utility.CreateResponse(request, HttpStatusCode.Created);
            return response;
        }



    }
}

