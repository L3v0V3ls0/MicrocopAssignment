using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityConstraints
{
    public class GeneralConstraints
    {
        public const int EmailMaxLength = 254;
        public const int MobileNumberMaxLength = 20;


        public const string EmailMessage = "Invalid email address";
        public const string PhoneMessage = "Invalid phone number format";
    }
}
