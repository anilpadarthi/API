﻿using AutoMapper;
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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateUserAsync(UserDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDbo = await _userRepository.GetUserByEmailAsync(request.Email);
                if (userDbo != null)
                {
                    response = Utility.CreateResponse("User is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    userDbo = _mapper.Map<User>(request);
                    userDbo.Status = (short)EnumStatus.Active;
                    userDbo.CreatedDate = DateTime.Now;
                    if (request.UserImageFile != null)
                    {
                        userDbo.UserImage = FileUtility.uploadImage(request.UserImageFile, FolderUtility.user);
                    }
                    userDbo.UserImage = userDbo.UserImage ?? "";
                    _userRepository.Add(userDbo);
                    await _userRepository.SaveChangesAsync();
                    await UpdateOrCreateUserDocuments(null, request.UserDocuments, userDbo.UserId);
                    response = Utility.CreateResponse(userDbo, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateUserAsync(UserDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDbo = await _userRepository.GetUserByEmailAsync(request.Email);
                if (userDbo != null && userDbo.UserId != request.UserId)
                {
                    response = Utility.CreateResponse("User name already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    userDbo = await _userRepository.GetUserByIdAsync(request.UserId);
                    _mapper.Map(request, userDbo);
                    userDbo.UpdatedDate = DateTime.Now;
                    userDbo.Locality = userDbo.Locality ?? "";
                    userDbo.Designation = userDbo.Designation ?? "";
                    if (request.UserImageFile != null)
                    {
                        userDbo.UserImage = FileUtility.uploadImage(request.UserImageFile, FolderUtility.user);
                    }
                    await _userRepository.SaveChangesAsync();
                    var savedDocuments = await _userRepository.GetUserDocumentsAsync(userDbo.UserId);
                    await UpdateOrCreateUserDocuments(savedDocuments, request.UserDocuments, userDbo.UserId);
                    response = Utility.CreateResponse(userDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }


        public async Task<CommonResponse> DeleteUserAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDBData = await _userRepository.GetUserByIdAsync(id);
                if (userDBData != null)
                {
                    userDBData.Status = (int)EnumStatus.Deleted;
                    userDBData.UpdatedDate = DateTime.Now;
                    await _userRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(userDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("User name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }


        public async Task<CommonResponse> GetUserByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _userRepository.GetUserDetailsAsync(id);
                if (!string.IsNullOrEmpty(result.user.UserImage))
                    result.user.UserImage = FileUtility.GetImagePath(FolderUtility.user, result.user.UserImage);
                if (result.userDocuments != null)
                {
                    result.userDocuments.ToList().ForEach(e => e.DocumentImage = FileUtility.GetImagePath(FolderUtility.userDocument, e.DocumentImage));
                }

                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _userRepository.GetUserByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetAllUsersAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _userRepository.GetAllUsersAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUsersByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _userRepository.GetUsersByPagingAsync(request);
                pageResult.TotalRecords = await _userRepository.GetTotalUserCountAsync(request);

                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateUserPasswordAsync(UserDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponse> AllocateAgentsToUserAsync(AllocateAgentDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                foreach (var id in request.agentIds)
                {
                    var existingAreaMap = await _userRepository.GetAgentMapByAgentIdAsync(id);
                    if (existingAreaMap != null)
                    {
                        existingAreaMap.IsActive = false;
                        existingAreaMap.ToDate = request.fromDate ?? DateTime.Now;
                        existingAreaMap.MappedDate = DateTime.Now;
                        await _userRepository.SaveChangesAsync();
                    }

                    UserMap umap = new UserMap();
                    umap.UserId = id;
                    umap.MonitorBy = request.managerId;
                    umap.FromDate = request.fromDate ?? new DateTime();
                    umap.IsActive = true;
                    umap.MappedDate = DateTime.Now;
                    _userRepository.Add(umap);
                    await _userRepository.SaveChangesAsync();
                }

                response = Utility.CreateResponse("Allocated successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeAllocateUsersToManagerAsync(int[] userIds, int managerId)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponse> GetAllAgentsToAllocateAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _userRepository.GetAllAgentsToAllocateAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> ViewUserAllocationHistorySync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _userRepository.ViewUserAllocationHistorySync(id);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAddressAsync(int userId,string shippingAddress)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDetails = await _userRepository.GetUserByIdAsync(userId);
                userDetails.Address = shippingAddress;
                await _userRepository.SaveChangesAsync();
                await CreateUserLog(userDetails);
                response = Utility.CreateResponse("Saved successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }

        public async Task<CommonResponse> SendActivationEmailAsync(int userId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _userRepository.GetUserDetailsAsync(userId);
                
                CommunicationHelper.UserPasswordResetEmail(result.user.UserId, result.user.UserName, result.user.Email );
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _userRepository);
            }
            return response;
        }


        public async Task<string> GenerateResetTokenAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null; // User not found
            }

            // Generate token (you can use any secure token generation mechanism)
            var token = Guid.NewGuid().ToString();

            // Store token and timestamp in the database
            var resetToken = new PasswordResetToken
            {
                UserId = user.UserId,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow
            };

            _userRepository.Add(resetToken);
            await _userRepository.SaveChangesAsync();

            // Send reset link email to the user
            var resetLink = $"https://yourfrontend.com/reset-password?token={token}";
            CommunicationHelper.UserPasswordResetEmail(user.UserId, user.UserName, user.Email);

            return token;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var resetToken = await _userRepository.GetPasswordResetToken(token);
            if (resetToken == null || resetToken.ExpiryTime < DateTime.UtcNow)
            {
                return false; // Token is invalid or expired
            }

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                return false; // Passwords do not match
            }

            var resetToken = await _userRepository.GetPasswordResetToken(token);
            if (resetToken == null || resetToken.ExpiryTime < DateTime.UtcNow)
            {
                return false; // Token is invalid or expired
            }

            var user = await _userRepository.GetUserByIdAsync(resetToken.UserId);
            if (user == null)
            {
                return false; // User not found
            }

            // Update the user's password (you should hash it before saving)
            user.Password= newPassword;
            await _userRepository.SaveChangesAsync();

            // Delete the reset token as it's been used
            _userRepository.Remove(resetToken);
            await _userRepository.SaveChangesAsync();

            return true;
        }



        private async Task CreateUserLog(User user)
        {
            var log = _mapper.Map<UserLog>(user);
            _userRepository.Add(log);
            await _userRepository.SaveChangesAsync();
        }

        private async Task CreateUserDocuments(List<UserDocumentDto> documentList, int userId)
        {
            foreach (var item in documentList)
            {
                var contact = _mapper.Map<UserDocument>(item);
                contact.Status = (int)EnumStatus.Active;
                contact.UserId = userId;
                _userRepository.Add(contact);
            }
            await _userRepository.SaveChangesAsync();
        }

        private async Task UpdateOrCreateUserDocuments(IEnumerable<UserDocument>? savedDocuments, List<UserDocumentDto> documentList, int userId)
        {
            if (savedDocuments != null)
            {
                foreach (var savedDoc in savedDocuments)
                {
                    var matchedDocument = documentList != null ? documentList.Where(w => w.UserDocumentId == savedDoc.UserDocumentId).FirstOrDefault() : null;
                    if (matchedDocument != null)
                    {
                        _mapper.Map(matchedDocument, savedDoc);
                        if (matchedDocument.DocumentImageFile != null)
                        {
                            savedDoc.DocumentImage = FileUtility.uploadImage(matchedDocument.DocumentImageFile, FolderUtility.userDocument);
                        }
                    }
                    else
                    {
                        // Mark saved document as inactive or deleted
                        savedDoc.Status = (int)EnumStatus.Deleted; // Assuming 0 indicates inactive/deleted
                    }
                }
            }
            if (documentList != null)
            {
                // Process incoming contacts that are new (not found in saved contacts)
                foreach (var item in documentList.Where(c => c.UserDocumentId == null || c.UserDocumentId == 0))
                {
                    var newDocument = _mapper.Map<UserDocument>(item);
                    newDocument.UserId = userId;
                    newDocument.Status = (int)EnumStatus.Active;
                    newDocument.CreatedDate = DateTime.Now;
                    newDocument.UpdatedDate = DateTime.Now;
                    if (item.DocumentImageFile != null)
                    {
                        newDocument.DocumentImage = FileUtility.uploadImage(item.DocumentImageFile, FolderUtility.userDocument);
                    }
                    newDocument.DocumentImage = newDocument.DocumentImage ?? "";
                    _userRepository.Add(newDocument);
                }
            }

            await _userRepository.SaveChangesAsync();
        }
    }
}
