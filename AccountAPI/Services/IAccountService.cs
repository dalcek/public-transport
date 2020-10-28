using AccountAPI.Models;
using Microsoft.AspNetCore.Http;
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
        Task<ServiceResponse<GetUserDTO>> Update(User user);
        Task<ServiceResponse<string>> UploadImage(HttpRequest httpContext);
        Task<ServiceResponse<GetUserDTO>> GetUser();
        Task<ServiceResponse<List<GetUserDTO>>> GetUnvalidatedUsers();
        ServiceResponse<Enums.UserType> GetUserType(int id);
        Task<ServiceResponse<string>> Validate(string email, bool accepted);
        Task<bool> UserExists(string email);
    }
}
