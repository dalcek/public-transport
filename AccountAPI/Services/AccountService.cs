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
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

      public object HttpPostedFile { get; private set; }

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

      public ServiceResponse<Enums.UserType> GetUserType(int id)
      {
         ServiceResponse<Enums.UserType> response = new ServiceResponse<Enums.UserType>();
         try
         {
            User user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
               response.Data = user.UserStatus == Enums.UserStatus.Accepted ? user.UserType : Enums.UserType.RegularUser;
            }
            else
            {
               response.Success = false;
               response.Message = "User with the given id is not found.";
            }
         }
         catch (Exception)
         {
            response.Success = false;
            response.Message = "User with the given id is not found.";
         }
         return response;
      }
      public async Task<ServiceResponse<string>> Login(string email, string password)
      {
         ServiceResponse<string> response = new ServiceResponse<string>();
         User user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
         if (user == null)
         {
            response.Success = false;
            response.Message = "Incorrect credentials.";
         }
         else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
         {
            response.Success = false;
            response.Message = "Incorrect credentials.";
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
      //TODO: add mapping, don't forget to add a photo separately, you can remove photo from add user dto
      public async Task<ServiceResponse<GetUserDTO>> Update(User request)
      {
         ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
         try
         {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (user != null)
            {
               // TODO: Password
               user.Email = request.Email;
               user.Name = request.Name;
               user.LastName = request.LastName;
               user.DateOfBirth = request.DateOfBirth;
               if (user.UserType != request.UserType)
               {
                  if (request.UserType != Enums.UserType.RegularUser)
                  {
                     user.UserStatus = Enums.UserStatus.InProcess;
                  }
                  user.UserType = request.UserType;
               }
               // If changed, user photo will be sent in other request
               //user.Photo = request.Photo;

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

      public async Task<ServiceResponse<GetUserDTO>> GetUser()
      {
         ServiceResponse<GetUserDTO> response = new ServiceResponse<GetUserDTO>();
         try
         {
            User user = await _context.Users.FirstAsync(user => user.Id == GetUserId());
            if (user == null) 
            {
               response.Success = false;
               response.Message = "User not found.";
            }
            response.Data = _mapper.Map<GetUserDTO>(user);
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<string>> UploadImage(HttpRequest httpRequest)
      {
         ServiceResponse<string> response = new ServiceResponse<string>();

         try
         {
            var userId = httpRequest.Form["id"];
            int id = Int32.Parse(userId.FirstOrDefault());
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
               response.Success = false;
               response.Message = "User not found.";
               return response;
            }
            var file = httpRequest.Form.Files[0];
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
               var fileExtension = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').Split('.').Last();
               var fileName = $"{id}.{fileExtension}";
               var fullPath = Path.Combine(pathToSave, fileName);
               var dbPath = Path.Combine(folderName, fileName);

               using ( var stream = new FileStream(fullPath, FileMode.Create))
               {
                  file.CopyTo(stream);
               }

               user.Photo = dbPath;
               // User needs to be validated again by controller
               user.UserStatus = Enums.UserStatus.InProcess;
               _context.Users.Update(user);
               await _context.SaveChangesAsync();
               response.Data = dbPath;
            }
         }
         catch(Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
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
