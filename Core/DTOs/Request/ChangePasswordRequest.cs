using Core.EntityConstraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Request
{
    public class ChangePasswordRequest
    {
        [Required]
        [StringLength(maximumLength: UserConstraints.PasswordMaxLength,
                        MinimumLength = UserConstraints.PasswordMinLength)]
        public string Password { get; set; }
    }
}
