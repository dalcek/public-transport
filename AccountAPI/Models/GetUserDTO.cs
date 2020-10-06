using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccountAPI.Models.Enums;

namespace AccountAPI.Models
{
    public class GetUserDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        //public DateTime DateOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string Photo { get; set; }
        //public UserType UserType { get; set; }
        public string UserType { get; set; }
        //public UserStatus UserStatus { get; set; }
        public string UserStatus { get; set; }
    }
}
