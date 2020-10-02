using AccountAPI.Data;
using AccountAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountAPI.Services
{
   public class AccountService : IAccountService
   {
      private readonly DataContext _context;
      private readonly IConfiguration _configuration;
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly IMapper _mapper;
      public AccountService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
      {
         _context = context;
         _configuration = configuration;
         _httpContextAccessor = httpContextAccessor;
         _mapper = mapper;
      }

      private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
      private string GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
      private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

      public async Task<ServiceResponse<string>> Login(string email, string password)
      {
         ServiceResponse<string> response = new ServiceResponse<string>();
         User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
         if (user == null)
         {
            response.Success = false;
            response.Message = "Login failed.";
         }
         else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
         {
            response.Success = false;
            response.Message = "Login failed.";
         }
         else
         {
            response.Data = CreateToken(user);
         }
         return response;
      }

      public async Task<ServiceResponse<int>> Register(User user, string password)
      {
         ServiceResponse<int> response = new ServiceResponse<int>();
         if (await UserExists(user.Email))
         {
            response.Success = false;
            response.Message = "User with this email already exists.";
            return response;
         }
         Data.Utility.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

         user.PasswordHash = passwordHash;
         user.PasswordSalt = passwordSalt;

         await _context.Users.AddAsync(user);
         await _context.SaveChangesAsync();

         response.Data = user.Id;
         return response;
      }
      public async Task<ServiceResponse<GetUserDTO>> Update(AddUserDTO request)
      {
         ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
         try
         {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (user != null)
            {
               user.Email = request.Email;
               user.Name = request.Name;
               user.LastName = request.LastName;
               user.DateOfBirth = request.DateOfBirth;
               user.Photo = request.Photo;
               user.UserType = request.UserType;
               _context.Users.Update(user);
               await _context.SaveChangesAsync();
               response.Data = _mapper.Map<GetUserDTO>(user);
            }
            else
            {
               response.Success = false;
               response.Message = "User not found.";
            }
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<string>> Delete(string password)
      {
         ServiceResponse<string> response = new ServiceResponse<string>();
         User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
         
         if (VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
         {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            response.Data = user.Email;
         }
         else
         {
            response.Success = false;
            response.Message = "Password not correct.";
         }
         return response;
      }
      public async Task<bool> UserExists(string email)
      {
         if (await _context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower()))
         {
            return true;
         }
         return false;
      }
      private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
      {
         using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
         {
               var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
               for (int i = 0; i < computedHash.Length; i++)
               {
                  if (computedHash[i] != passwordHash[i])
                  {
                     return false;
                  }
               }
               return true;
         }
      }
      private string CreateToken(User user)
      {
         List<Claim> claims = new List<Claim>
         {
         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
         new Claim(ClaimTypes.Email, user.Email),
         new Claim(ClaimTypes.Role, user.Role)
         };

         SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
         );

         SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

         SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
         {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
         };

         JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
         SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

         return tokenHandler.WriteToken(token);
      }
   }
}
