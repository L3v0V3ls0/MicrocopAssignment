using Core.EntityConstraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Request
{
    public class ValidatePasswordRequest
    {
        [Required]
        [StringLength(maximumLength: UserConstraints.UserNameMaxLength,
                        MinimumLength = UserConstraints.UserNameMinLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(maximumLength: UserConstraints.PasswordMaxLength,
                        MinimumLength = UserConstraints.PasswordMinLength)]
        public string Password { get; set; }
    }
}
