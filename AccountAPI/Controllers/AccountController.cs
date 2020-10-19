using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AccountAPI.Models;
using AccountAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountAPI.Controllers
{
   [Authorize]
   [Route("[controller]")]
   [ApiController]
   public class AccountController : ControllerBase
   {
      private readonly IAccountService _accountService;
      private readonly IMapper _mapper;
      public AccountController(IAccountService accountService, IMapper mapper)
      {
         _accountService = accountService;
         _mapper = mapper;
      }

      [AllowAnonymous]
      [HttpPost("register")]
      public async Task<IActionResult> Register(AddUserDTO request)
      {
         ServiceResponse<int> response = await _accountService.Register(
            _mapper.Map<User>(request), request.Password
         );

         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpPost("login")]
      public async Task<IActionResult> Login(LoginUserDTO request)
      {
         ServiceResponse<string> response = await _accountService.Login(request.Email, request.Password);
         if (!response.Success)
         {
               return BadRequest(response);
         }
         return Ok(response);
      }

      [HttpGet]
      public async Task<IActionResult> GetUser()
      {
         ServiceResponse<GetUserDTO> response = await _accountService.GetUser();
         if (!response.Success)
         {
               return BadRequest(response);
         }
         return Ok(response);
      }

      [HttpPut("update")]
      public async Task<IActionResult> Update(AddUserDTO request)
      {
         User user = _mapper.Map<User>(request);
         ServiceResponse<GetUserDTO> response = await _accountService.Update(user);
         if (!response.Success)
         {
            return NotFound(response);
         }
         return Ok(response);
      }

      [HttpPost("delete")]
      public async Task<IActionResult> Delete(LoginUserDTO request)
      {
         ServiceResponse<string> response = await _accountService.Delete(request.Password);

         if (!response.Success)
         {
            return NotFound(response);
         }
         return Ok(response);
      }

      [HttpPost("uploadImage"), DisableRequestSizeLimit]
      public async Task<IActionResult> UploadImage()
      {
         var httpRequest = Request;
         ServiceResponse<string> response = await _accountService.UploadImage(Request);

         if (!response.Success)
         {
            return BadRequest();
         }

         return Ok(response);
      }
   }
}
