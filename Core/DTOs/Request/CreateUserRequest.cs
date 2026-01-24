using Core.EntityConstraints;
using System.ComponentModel.DataAnnotations;
namespace Core.DTOs.Request
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(  maximumLength: UserConstraints.UserNameMaxLength, 
                        MinimumLength = UserConstraints.UserNameMinLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(  maximumLength: UserConstraints.FullNameMaxLength,
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

        [Required]
        [StringLength(  maximumLength: UserConstraints.PasswordMaxLength,
                        MinimumLength = UserConstraints.PasswordMinLength)]
        public string Password { get; set; }
    }
}
