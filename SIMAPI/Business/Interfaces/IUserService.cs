﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IUserService
    {
        Task<CommonResponse> GetUserByIdAsync(int id);
        Task<CommonResponse> GetUserByNameAsync(string name);
        Task<CommonResponse> GetAllUsersAsync();
        Task<CommonResponse> GetUsersByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateUserAsync(UserDto request);
        Task<CommonResponse> UpdateUserAsync(UserDto request);
        Task<CommonResponse> DeleteUserAsync(int id);
        Task<CommonResponse> UpdateUserPasswordAsync(UserDto request);
        Task<CommonResponse> AllocateUsersToManagerAsync(int[] userIds, int managerId);
        Task<CommonResponse> DeAllocateUsersToManagerAsync(int[] userIds, int managerId);
    }
}