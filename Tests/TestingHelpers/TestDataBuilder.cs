using Core.DTOs.Request;
using Core.Entities;
using System;
using System.Security.Cryptography;

namespace Tests.TestingHelpers
{
    public static class TestDataBuilder
    {
        public static CreateUserRequest CreateUserRequest(
            string? userName = null,
            string? fullName = null,
            string? email = null,
            string? password = null)
        {
            return new CreateUserRequest
            {
                UserName = userName ?? "testuser",
                FullName = fullName ?? "Test User",
                Email = email ?? "test@example.com",
                MobileNumber = "+1234567890",
                Language = "en",
                Culture = "en-US",
                Password = password ?? "Password123!"
            };
        }

        public static UpdateUserRequest UpdateUserRequest(
            string? userName = null,
            string? fullName = null,
            string? email = null)
        {
            return new UpdateUserRequest
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                MobileNumber = null,
                Language = null,
                Culture = null
            };
        }

        public static User CreateUser(Guid? id = null,string? userName = null,bool isActive = true, string? passwordHash = null)
        {
            var userId = id ?? Guid.NewGuid();
            var hash = passwordHash ?? BCrypt.Net.BCrypt.HashPassword("Password123!");

            return new User(
                userId.ToString(),
                userName ?? "testuser",
                "Test User",
                "test@example.com",
                "+1234567890",
                "en",
                "en-US",
                hash,
                DateTime.UtcNow.ToString("O"),
                DateTime.UtcNow.ToString("O"),
                isActive ? 1 : 0
            );
        }

        public static string GenerateRandom(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var randomBytes = RandomNumberGenerator.GetBytes(length);

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[randomBytes[i] % chars.Length];
            }

            return new string(result);
        }
        public static string GenerateRandomNumberString(int length)
        {
            const string chars = "0123456789";
            var randomBytes = RandomNumberGenerator.GetBytes(length);

            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[randomBytes[i] % chars.Length];
            }

            return new string(result);
        }
    }
}
