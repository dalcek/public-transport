using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAPI.Models
{
    public class Enums
    {
        public enum UserType
        {
            RegularUser = 1,
            Student,
            Retired
        }

        public enum UserStatus
        {
            InProcess = 1,
            Accepted,
            Denied
        }
    }
}
