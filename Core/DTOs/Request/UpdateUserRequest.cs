using Core.EntityConstraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Request
{
    public class UpdateUserRequest
    {
        [StringLength(maximumLength: UserConstraints.UserNameMaxLength,
                        MinimumLength = UserConstraints.UserNameMinLength)]
        public string? UserName { get; set; } = null;

        [StringLength(maximumLength: UserConstraints.FullNameMaxLength,
                        MinimumLength = UserConstraints.FullNameMinLength)]
        public string? FullName { get; set; } = null;

        [EmailAddress(ErrorMessage = GeneralConstraints.EmailMessage)]
        [MaxLength(GeneralConstraints.EmailMaxLength)]
        public string? Email { get; set; } = null;

        [MaxLength(GeneralConstraints.MobileNumberMaxLength)]
        [Phone(ErrorMessage = GeneralConstraints.PhoneMessage)]
        public string? MobileNumber { get; set; } = null;

        [MaxLength(UserConstraints.LanguageMaxLength)]
        public string? Language { get; set; } = null;

        [MaxLength(UserConstraints.CultureMaxLength)]
        public string? Culture { get; set; } = null;

    }
}
