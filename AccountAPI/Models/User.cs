using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static AccountAPI.Models.Enums;

namespace AccountAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Photo { get; set; }
        [Required]
        public UserType UserType { get; set; }
        public UserStatus UserStatus { get; set; } = Enums.UserStatus.InProcess;
        [Required]
        public string Role { get; set; }
    }
}
