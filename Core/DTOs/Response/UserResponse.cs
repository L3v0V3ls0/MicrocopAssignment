using Core.Entities;
using Core.EntityConstraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(maximumLength: UserConstraints.UserNameMaxLength,
                        MinimumLength = UserConstraints.UserNameMinLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(maximumLength: UserConstraints.FullNameMaxLength,
                        MinimumLength = UserConstraints.FullNameMinLength)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = GeneralConstraints.EmailMessage)]
        [MaxLength(GeneralConstraints.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        [MaxLength(GeneralConstraints.MobileNumberMaxLength)]
        [Phone(ErrorMessage = GeneralConstraints.PhoneMessage)]
        public string MobileNumber { get; set; }

        [MaxLength(UserConstraints.LanguageMaxLength)]
        public string Language { get; set; } = "en";

        [MaxLength(UserConstraints.CultureMaxLength)]
        public string Culture { get; set; } = "en-US";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public UserResponse(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            FullName = user.FullName;
            Email = user.Email;
            MobileNumber = user.MobileNumber;
            Language = user.Language;
            Culture = user.Culture;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
        }
    }
}
