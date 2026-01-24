using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityConstraints
{
    public class UserConstraints
    {
        // User validation
        public const int UserNameMinLength = 3;
        public const int UserNameMaxLength = 50;

        public const int FullNameMinLength = 2;
        public const int FullNameMaxLength = 100;

        public const int LanguageMaxLength = 3;
        public const int CultureMaxLength = 5;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 100;

        public const int PasswordHashMaxLength = 256;
    }
}
