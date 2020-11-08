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
               Console.WriteLine("test1");
               rpcResponse = System.Text.Json.JsonSerializer.Deserialize<ServiceResponse<int>>(res);
               Console.WriteLine("test2");
               if (rpcResponse.Success)
               {
                  Console.WriteLine("test3");
                  userType = (Enums.UserType) rpcResponse.Data;
               }
               Console.WriteLine("test4");
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

      public async Task<ServiceResponse<PricelistDTO>> CreatePricelist(PricelistDTO pricelist)
      {
         ServiceResponse<PricelistDTO> response = new ServiceResponse<PricelistDTO>();
         try
         {
            Pricelist old = await _context.Pricelists.FirstOrDefaultAsync(p => p.Active);
            old.Active = false;
            _context.Pricelists.Update(old);
            Pricelist newPricelist = new Pricelist { From = DateTime.Now, To = Convert.ToDateTime(pricelist.To), Active = true };
            await _context.Pricelists.AddAsync(newPricelist);
            await _context.SaveChangesAsync();

            Item hour = await _context.Items.FirstOrDefaultAsync(i => i.TicketType == Enums.TicketType.HourTicket);
            Item day = await _context.Items.FirstOrDefaultAsync(i => i.TicketType == Enums.TicketType.DayTicket);
            Item month = await _context.Items.FirstOrDefaultAsync(i => i.TicketType == Enums.TicketType.MonthTicket);
            Item year = await _context.Items.FirstOrDefaultAsync(i => i.TicketType == Enums.TicketType.YearTicket);

            await _context.PricelistItems.AddAsync(new PricelistItem { Price = pricelist.HourPrice, PricelistId = newPricelist.Id, ItemId = hour.Id });
            await _context.PricelistItems.AddAsync(new PricelistItem { Price = pricelist.DayPrice, PricelistId = newPricelist.Id, ItemId = day.Id });
            await _context.PricelistItems.AddAsync(new PricelistItem { Price = pricelist.MonthPrice, PricelistId = newPricelist.Id, ItemId = month.Id });
            await _context.PricelistItems.AddAsync(new PricelistItem { Price = pricelist.YearPrice, PricelistId = newPricelist.Id, ItemId = year.Id });

            await _context.SaveChangesAsync();
            response.Data = pricelist;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }

         return response;
      }

      public async Task<ServiceResponse<PricelistDTO>> UpdatePricelist(PricelistDTO pricelist)
      {
         ServiceResponse<PricelistDTO> response = new ServiceResponse<PricelistDTO>();
         PricelistItem pricelistItem;
         try
         {
            Pricelist pl = await _context.Pricelists.FirstOrDefaultAsync(p => p.Active == true);
            pl.From = Convert.ToDateTime(pricelist.From);
            pl.To = Convert.ToDateTime(pricelist.To);
            _context.Pricelists.Update(pl);

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pl.Id 
               && pi.Item.TicketType == Enums.TicketType.HourTicket);
            pricelistItem.Price = pricelist.HourPrice;
            _context.PricelistItems.Update(pricelistItem);

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pl.Id 
               && pi.Item.TicketType == Enums.TicketType.DayTicket);
            pricelistItem.Price = pricelist.DayPrice;
            _context.PricelistItems.Update(pricelistItem);

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pl.Id 
               && pi.Item.TicketType == Enums.TicketType.MonthTicket);
            pricelistItem.Price = pricelist.MonthPrice;
            _context.PricelistItems.Update(pricelistItem);

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pl.Id 
               && pi.Item.TicketType == Enums.TicketType.YearTicket);
            pricelistItem.Price = pricelist.YearPrice;
            _context.PricelistItems.Update(pricelistItem);
            await _context.SaveChangesAsync();

            response.Data = pricelist;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }
         return response;
      }

      public async Task<ServiceResponse<PricelistDTO>> GetPricelist()
      {
         ServiceResponse<PricelistDTO> response = new ServiceResponse<PricelistDTO>();
         PricelistDTO temp = new PricelistDTO();
         PricelistItem pricelistItem;
         try
         {
            Pricelist pricelist = await _context.Pricelists.FirstOrDefaultAsync(p => p.Active == true);
            
            temp.From = pricelist.From.ToString();
            temp.To = pricelist.To.ToString();
            
            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pricelist.Id 
               && pi.Item.TicketType == Enums.TicketType.HourTicket);
            temp.HourPrice = (int) pricelistItem.Price;

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pricelist.Id 
               && pi.Item.TicketType == Enums.TicketType.DayTicket);
            temp.DayPrice = (int) pricelistItem.Price;

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pricelist.Id 
               && pi.Item.TicketType == Enums.TicketType.MonthTicket);
            temp.MonthPrice = (int) pricelistItem.Price;

            pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pricelist.Id 
               && pi.Item.TicketType == Enums.TicketType.YearTicket);
            temp.YearPrice = (int) pricelistItem.Price;
            
            response.Data = temp;
         }
         catch (Exception e)
         {
            response.Success = false;
            response.Message = e.Message;
         }

         return response;
      }

      public async Task<ServiceResponse<bool>> ValidateTicket(int id)
      {
         ServiceResponse<bool> response = new ServiceResponse<bool>();

         try
         {
            Ticket ticket = await _context.Tickets.Include(t => t.PricelistItem).ThenInclude(pi => pi.Item).FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
               response.Success = false;
               response.Message = "Ticket with the given ID was not found.";
               return response;
            }

            if (ticket.PricelistItem.Item.TicketType == Enums.TicketType.HourTicket)
            {
               if ((DateTime.Now.Ticks - ticket.IssueTime.Ticks) < 36000000000)
               {
                  response.Data = true;
               }
            }
            else if (ticket.PricelistItem.Item.TicketType == Enums.TicketType.DayTicket)
            {
               if (ticket.IssueTime.Year == DateTime.Now.Year && 
                  ticket.IssueTime.Month == DateTime.Now.Month &&
                  ticket.IssueTime.Day == DateTime.Now.Day)
               {
                  response.Data = true;
               }
            }
            else if (ticket.PricelistItem.Item.TicketType == Enums.TicketType.MonthTicket)
            {
               if (ticket.IssueTime.Year == DateTime.Now.Year && 
                  ticket.IssueTime.Month == DateTime.Now.Month)
               {
                  response.Data = true;
               }
            }
            else if (ticket.PricelistItem.Item.TicketType == Enums.TicketType.YearTicket)
            {
               if (ticket.IssueTime.Year == DateTime.Now.Year)
               {
                  response.Data = true;
               }
            }
            response.Data = false;
            ticket.Valid = false;
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
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