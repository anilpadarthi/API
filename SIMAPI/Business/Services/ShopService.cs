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
using System.Text.Json;


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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
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
                            shopDbo.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.shop);
                        }
                        shopDbo.OldShopId = await _shopRepository.GetNextOldShopIdAsync() + 1;
                        _shopRepository.Add(shopDbo);
                        await _shopRepository.SaveChangesAsync();
                        await CreateShopLog(shopDbo);
                        await CreateShopAgreement(request, shopDbo.ShopId);
                        await CreateShopContacts(request.ShopContacts, shopDbo.ShopId);
                        response = Utility.CreateResponse(shopDbo, HttpStatusCode.Created);
                        await transaction.CommitAsync();
                        CommunicationHelper.SendWelcomeEmail(shopDbo.ShopId, shopDbo.ShopName, request.ShopEmail, shopDbo.Password, request.ShopOwnerName);
                        CommunicationHelper.SendRegistrationEmail(shopDbo.ShopId, shopDbo.ShopName, request.ShopEmail, shopDbo.Password, request.ShopOwnerName);
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    var json = JsonSerializer.Serialize(request);
                    response = response.HandleException(ex, _shopRepository, json);
                }
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(ShopDto request)
        {
            CommonResponse response = new CommonResponse();
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
                    var sContacts = await _shopRepository.GetShopContactsAsync(request.ShopId);
                    var sAgreement = await _shopRepository.GetShopAgreementAsync(request.ShopId);
                    _mapper.Map(request, shopDbo);
                    shopDbo.UpdatedDate = DateTime.Now;
                    if (request.ImageFile != null)
                    {
                        shopDbo.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.shop);
                    }
                    await _shopRepository.SaveChangesAsync();
                    await CreateShopLog(shopDbo);
                    await UpdateAndCreateShopAgreement(sAgreement, request, shopDbo.ShopId);
                    await UpdateOrCreateShopContacts(sContacts, request.ShopContacts, shopDbo.ShopId);
                    response = Utility.CreateResponse(shopDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                var json = JsonSerializer.Serialize(request);
                response = response.HandleException(ex, _shopRepository,json);
            }
            return response;
        }


        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopDetailsAsync(id);
                if (!string.IsNullOrEmpty(result.shop.Image))
                    result.shop.Image = FileUtility.GetImagePath(FolderUtility.shop, result.shop.Image);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name, string postCode)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopByNameAsync(name, postCode);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetAllAsync(int? areaId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetAllShopsAsync(areaId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _shopRepository.GetShopsByPagingAsync(request);
                pageResult.TotalRecords = await _shopRepository.GetTotalShopsCountAsync(request);

                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> ShopVisitAsync(ShopVisitRequestmodel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.ImageFile != null)
                {
                    request.ReferenceImage = FileUtility.uploadImage(request.ImageFile, FolderUtility.shopVisit);
                }
                var result = await _shopRepository.ShopVisitAsync(request);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                var json = JsonSerializer.Serialize(request);
                response = response.HandleException(ex, _shopRepository, json);
            }
            return response;

        }

        public async Task<CommonResponse> GetShopVisitHistoryAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopVisitHistoryAsync(shopId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopAgreementHistoryAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopAgreementHistoryAsync(shopId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopWalletAmountAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopWalletAmountAsync(shopId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopWalletHistoryAsync(int shopId, string walletType)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopWalletHistoryAsync(shopId, walletType);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopAddressDetailsAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopAddressDetailsAsync(shopId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> SendActivationEmailAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopDetailsAsync(shopId);
                var shopDbo = result.shop;
                var shopContacts = result.shopContacts;
                CommunicationHelper.SendRegistrationEmail(shopDbo.ShopId, shopDbo.ShopName, shopDbo.ShopEmail, shopDbo.Password, "");
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAddressAsync(ShippingAddressDetails request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var shop = await _shopRepository.GetShopByIdAsync(request.ShopId);
                shop.ShopName = request.ShopName;
                shop.AddressLine1 = request.AddressLine1;
                shop.DeliveryInstructions = request.DeliveryInstructions;
                shop.ShopOwnerName = request.ShopOwnerName;
                shop.ShopEmail = request.ShopEmail;
                shop.ShopPhone = request.ShopPhone;
                await _shopRepository.SaveChangesAsync();
                response = Utility.CreateResponse("Saved successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        private async Task CreateShopLog(Shop shop)
        {
            var log = _mapper.Map<ShopLog>(shop);
            _shopRepository.Add(log);
            await _shopRepository.SaveChangesAsync();
        }

        private async Task CreateShopAgreement(ShopDto request, int shopId)
        {
            if (request.AgreementFrom.HasValue && request.AgreementTo.HasValue)
            {
                var shopAgreement = new ShopAgreement()
                {
                    AgreementNotes = request.AgreementNotes,
                    FromDate = request.AgreementFrom.Value,
                    ToDate = request.AgreementTo.Value,
                    ShopId = shopId,
                    AgreementBy = request.CreatedBy.Value,
                    Status = (int)EnumStatus.Hold,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                _shopRepository.Add(shopAgreement);

                await _shopRepository.SaveChangesAsync();
            }
        }

        private async Task UpdateAndCreateShopAgreement(ShopAgreement savedAgreement, ShopDto request, int shopId)
        {
            if (savedAgreement != null)
            {
                if (savedAgreement.FromDate != request.AgreementFrom && savedAgreement.ToDate != request.AgreementTo)
                {
                    savedAgreement.Status = (int)EnumStatus.InActive;

                    var shopAgreement = new ShopAgreement()
                    {
                        AgreementNotes = request.AgreementNotes,
                        FromDate = Convert.ToDateTime(request.AgreementFrom),
                        ToDate = Convert.ToDateTime(request.AgreementTo),
                        ShopId = shopId,
                        AgreementBy = request.CreatedBy.Value,
                        Status = (int)EnumStatus.Hold,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };
                    _shopRepository.Add(shopAgreement);
                }
                else
                {
                    savedAgreement.AgreementNotes = request.AgreementNotes;
                }
            }
            else
            {
                var shopAgreement = new ShopAgreement()
                {
                    AgreementNotes = request.AgreementNotes,
                    FromDate = Convert.ToDateTime(request.AgreementFrom),
                    ToDate = Convert.ToDateTime(request.AgreementTo),
                    ShopId = shopId,
                    AgreementBy = request.CreatedBy.Value,
                    Status = (int)EnumStatus.Hold,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                _shopRepository.Add(shopAgreement);
            }
            await _shopRepository.SaveChangesAsync();
        }


        private async Task CreateShopContacts(ShopContactDto[] contacts, int shopId)
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
                await _shopRepository.SaveChangesAsync();
            }
        }

        private async Task UpdateOrCreateShopContacts(IEnumerable<ShopContact> savedContacts, ShopContactDto[] contacts, int shopId)
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
                        // Update other fields as needed
                    }
                    else
                    {
                        // Mark saved contact as inactive or deleted
                        savedContact.Status = (int)EnumStatus.Deleted; // Assuming 0 indicates inactive/deleted
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
                    _shopRepository.Add(newContact);
                }
            }

            await _shopRepository.SaveChangesAsync();
        }

        public async Task<CommonResponse> GetShopCommissionChequesAsync(int shopId, string mode)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopCommissionChequesAsync(shopId, mode);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopCommissionChequeAsync(int sno)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _shopRepository.GetShopCommissionChequeAsync(sno);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateShopCommissionChequeAsync(int sno, string chequeNumber)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var commissionCheque = await _shopRepository.GetShopCommissionChequeAsync(sno);
                commissionCheque.ChequeNumber = chequeNumber;
                commissionCheque.ModifiedDate = DateTime.Now;
                await _shopRepository.SaveChangesAsync();
                response = Utility.CreateResponse(commissionCheque, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteShopCommissionChequeAsync(int sno)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var commissionCheque = await _shopRepository.GetShopCommissionChequeAsync(sno);
                commissionCheque.IsDelete = true;
                commissionCheque.ModifiedDate = DateTime.Now;
                await _shopRepository.SaveChangesAsync();
                response = Utility.CreateResponse(commissionCheque, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GlobalShopSearchAsync(GetLookupRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var shopList = await _shopRepository.GlobalShopSearchAsync(request);

                response = Utility.CreateResponse(shopList, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _shopRepository);
            }
            return response;
        }
    }
}
