using AccountAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAPI.Services
{
    public interface IAccountService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<string>> Delete(string password);
        Task<ServiceResponse<GetUserDTO>> Update(AddUserDTO user);
        Task<bool> UserExists(string email);
    }
}
