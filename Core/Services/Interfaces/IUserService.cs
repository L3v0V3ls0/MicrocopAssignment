using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest dto);
        Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest dto);
        Task DeleteUserAsync(Guid id);
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<UserResponse?> GetUserByUserNameAsync(string userName);
        Task<bool> ValidatePasswordAsync(string userName, string password);
        Task<UserResponse> ChangePasswordAsync(Guid id, string newPassword);
    }
}
