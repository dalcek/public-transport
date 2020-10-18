using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TicketAPI.Models;
using TicketAPI.Services;

namespace TicketAPI.Controllers
{
   [Authorize]
   [Route("[controller]")]
   [ApiController]
   public class TicketController : ControllerBase
   {
      private readonly ITicketService _ticketService;

      public TicketController(ITicketService ticketService)
      {
          _ticketService = ticketService;
      }

      [AllowAnonymous]
      [HttpGet("test")]
      public IActionResult Test()
      {
         return Ok("test");
      }

      [AllowAnonymous]
      [HttpPost("createTicket")]
      public async Task<IActionResult> CreateTicket(CreateTicketDTO data)
      {
         ServiceResponse<AddedTicketDTO> response = new ServiceResponse<AddedTicketDTO>();
         ServiceResponse<int> price = await _ticketService.GetPrice(data.TicketType);
         if (!price.Success)
         {
            response.Message = price.Message;
            response.Success = price.Success;
            return BadRequest(response);
         }
         response = await _ticketService.CreateTicket(data.TicketType, price.Data);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         // TODO: Send email data.Email
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpGet("getAllPrices")]
      public async Task<IActionResult> GetAllPrices()
      {
         ServiceResponse<List<double>> response = await _ticketService.GetAllPrices();
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpGet("getPrice")]
      public async Task<IActionResult> GetPrice(string ticketType)
      {
         ServiceResponse<int> response = await _ticketService.GetPrice(ticketType);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpGet("getCoefficients")]
      public async Task<IActionResult> GetCoefficients()
      {
         ServiceResponse<List<Coefficient>> response = await _ticketService.GetCoefficients();

         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }
   }
}