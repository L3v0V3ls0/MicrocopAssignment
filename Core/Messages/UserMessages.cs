using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Errors
{
    public class UserMessages
    {
        public const string UsernameExists = "UserName already exists.";
        public const string NoUserToDelete = "User not found or already deleted.";
        public const string NoUser = "User not found.";
        public const string InvalidUsernamePassword = "Invalid username or password.";
        public const string PasswordOK = "Password is valid.";
    }
}
