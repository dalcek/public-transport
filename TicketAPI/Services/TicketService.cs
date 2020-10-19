using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Data;
using TicketAPI.Models;
using TicketAPI.RabbitMQClient;

namespace TicketAPI.Services
{
   public class TicketService : ITicketService
   {
      private readonly DataContext _context;
      private readonly IHttpContextAccessor _httpContextAccessor;
      private readonly RpcClient _rpcClient;
      public TicketService(DataContext context, IHttpContextAccessor httpContextAccessor, RpcClient rpcClient)
      {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
         _rpcClient = rpcClient;
      }

      private int GetUserId()
      {
         if (int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int id))
         {
            return id;
         }
         return -1;
      }

      public async Task<ServiceResponse<AddedTicketDTO>> CreateTicket(string ticketType, int ticketPrice, string email)
      {
         ServiceResponse<AddedTicketDTO> response = new ServiceResponse<AddedTicketDTO>();
         Enums.TicketType type = (Enums.TicketType) Enum.Parse(typeof(Enums.TicketType), ticketType);
         PricelistItem pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.Pricelist.Active == true && pi.Item.TicketType == type);
         Ticket ticket = new Ticket()
         {
            IssueTime = DateTime.Now,
            PricelistItemId = pricelistItem.Id,
            Valid = true,
            Price = (double) ticketPrice
         };
         int userId = GetUserId();
         if (userId != -1)
         {
            ticket.UserId = userId;
         }
         try
         {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            response.Data = new AddedTicketDTO(){ Id = ticket.Id, Price = ticket.Price};
            Utility.SendEmail(email, "Ticket purchase confirmation", "You have successfully purchased a ticket. Ticket id: " + ticket.Id);
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<int>> DeleteTicket(int id)
      {
         ServiceResponse<int> response = new ServiceResponse<int>();
         Ticket ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
         
         if (ticket != null)
         {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            response.Data = ticket.Id;
         }
         else
         {
            response.Success = false;
            response.Message = "Ticket with given id not found.";
         }
         return response;
      }

      public async Task<ServiceResponse<List<double>>> GetAllPrices()
      {
         ServiceResponse<List<double>> response = new ServiceResponse<List<double>>();

         try
         {
            Pricelist pricelist = await _context.Pricelists.FirstOrDefaultAsync(p => p.Active == true);
            if (pricelist != null)
            {
               response.Data = await _context.PricelistItems.Where(pi => pi.PricelistId == pricelist.Id).Select(pi => pi.Price).ToListAsync();
            }
            else
            {
               response.Success = false;
               response.Message = "No active pricelists were found.";
            }
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }
      public async Task<ServiceResponse<int>> GetPrice(string ticketType)
      {
         ServiceResponse<int> response = new ServiceResponse<int>();
         ServiceResponse<int> rpcResponse = new ServiceResponse<int>();

         int userId = GetUserId();
         Enums.UserType userType = Enums.UserType.RegulerUser;
         try
         {
            if (userId != -1)
            {
               // RPC call to account service to get user type
               var res = await _rpcClient.CallAsync(userId.ToString());
               rpcResponse = System.Text.Json.JsonSerializer.Deserialize<ServiceResponse<int>>(res);
               if (rpcResponse.Success)
               {
                  userType = (Enums.UserType) rpcResponse.Data;
               }
            }
         }
         catch (Exception)
         {
            response.Message = "Possible discount couldn't be processed. Price shown is without discount.\n";
         }

         try
         {
            Enums.TicketType type = (Enums.TicketType) Enum.Parse(typeof(Enums.TicketType), ticketType);
            PricelistItem pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.Pricelist.Active == true && pi.Item.TicketType == type);
            Coefficient coef = await _context.Coefficients.FirstOrDefaultAsync(c => c.UserType == userType);
            response.Data = (int) Math.Round(pricelistItem.Price * coef.Value);
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<List<Coefficient>>> GetCoefficients()
      {
         ServiceResponse<List<Coefficient>> response = new ServiceResponse<List<Coefficient>>();
         try
         {
            response.Data = await _context.Coefficients.ToListAsync();
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }
   }
}