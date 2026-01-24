using Core.DTOs.Request;
using Core.DTOs.Response;
using Core.Entities;
using Core.Errors;
using Core.Services.Interfaces;
using Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        // Create user
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest dto)
        {
            // Unique username check
            var existing = await _userRepo.GetByUserNameAsync(dto.UserName);
            if (existing != null && existing.IsActive == true)
                throw new Exception(UserMessages.UsernameExists);

            var user = new User(dto);

            user = await _userRepo.AddAsync(user);

            return new UserResponse(user);
        }
        // Soft-delete a user
        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null || !user.IsActive)
                throw new Exception(UserMessages.NoUserToDelete);

            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(user);
        }

        public async Task<UserResponse?> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return user?.IsActive == true ? new UserResponse(user) : null;
        }

        // Get user by username
        public async Task<UserResponse?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepo.GetByUserNameAsync(userName);
            return user?.IsActive == true ? new UserResponse(user) : null;
        }

        // Update user fields
        public async Task<UserResponse> UpdateUserAsync(Guid id, UpdateUserRequest dto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null || !user.IsActive)
                throw new Exception(UserMessages.NoUser);
            //update feilds if changed
            user.ApplyUpdate(dto);

            await _userRepo.UpdateAsync(user);

            return new UserResponse(user);
        }


        // Validate password for a user
        public async Task<bool> ValidatePasswordAsync(string userName, string password)
        {
            var user = await _userRepo.GetByUserNameAsync(userName);
            if (user == null || !user.IsActive)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        // Change password from user
        public async Task<UserResponse> ChangePasswordAsync(Guid id, string newPassword)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null || !user.IsActive)
                throw new Exception(UserMessages.NoUser);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepo.UpdateAsync(user);

            return new UserResponse(user);
        }
    }
}