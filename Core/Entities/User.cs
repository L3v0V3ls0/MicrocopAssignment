using Core.DTOs.Request;
using Core.EntityConstraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [StringLength(maximumLength: UserConstraints.UserNameMaxLength,
                        MinimumLength = UserConstraints.UserNameMinLength)]

        public string UserName { get; set; }

        [StringLength(maximumLength: UserConstraints.FullNameMaxLength,
                        MinimumLength = UserConstraints.FullNameMinLength)]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = GeneralConstraints.EmailMessage)]
        [MaxLength(GeneralConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [MaxLength(GeneralConstraints.MobileNumberMaxLength)]
        [Phone(ErrorMessage = GeneralConstraints.PhoneMessage)]
        public string MobileNumber { get; set; }

        [MaxLength(UserConstraints.LanguageMaxLength)]
        public string Language { get; set; } = "en";

        [MaxLength(UserConstraints.CultureMaxLength)]
        public string Culture { get; set; } = "en-US";

        [MaxLength(UserConstraints.PasswordHashMaxLength)]
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public User(string id, string userName, string fullName, string email, string mobileNumber, 
                    string language, string culture, string passwordHash, string createdAt, string updatedAt,
                    long isActive)
        {
            Id = Guid.Parse(id);
            UserName = userName;
            FullName = fullName;
            Email = email;
            MobileNumber = mobileNumber;
            Language = language;
            Culture = culture;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.Parse(createdAt);
            UpdatedAt = DateTime.Parse(updatedAt); ;
            IsActive = isActive == 1;
        }
        public User(CreateUserRequest dto)
        {
            Id = Guid.NewGuid();
            UserName = dto.UserName;
            FullName = dto.FullName;
            Email = dto.Email;
            MobileNumber = dto.MobileNumber;
            Language = dto.Language;
            Culture = dto.Culture;
            PasswordHash = HashPassword(dto.Password);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        // Method to update existing user from UpdateUserRequest
        public void ApplyUpdate(UpdateUserRequest dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.UserName))
                UserName = dto.UserName;

            if (!string.IsNullOrWhiteSpace(dto.FullName))
                FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.MobileNumber))
                MobileNumber = dto.MobileNumber;

            if (!string.IsNullOrWhiteSpace(dto.Language))
                Language = dto.Language;

            if (!string.IsNullOrWhiteSpace(dto.Culture))
                Culture = dto.Culture;

            UpdatedAt = DateTime.UtcNow;
        }

        // Private helper for hashing passwords
        private static string HashPassword(string password)
        {
            // Use a secure hashing algorithm like BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
