using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TicketAPI.Data;
using TicketAPI.Models;

namespace TicketAPI.Services
{
   public class TicketService : ITicketService
   {
      private readonly DataContext _context;
      private readonly IHttpContextAccessor _httpContextAccessor;
      public TicketService(DataContext context, IHttpContextAccessor httpContextAccessor)
      {
         _context = context;
         _httpContextAccessor = httpContextAccessor;
      }

      public Task<ServiceResponse<AddedTicketDTO>> AddTicket(string ticketType)
      {
         throw new NotImplementedException();
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
         try
         {
            Enums.TicketType type = (Enums.TicketType) Enum.Parse(typeof(Enums.TicketType), ticketType);
            //Pricelist pricelist = await _context.Pricelists.FirstOrDefaultAsync(p => p.Active == true);
            //PricelistItem pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.PricelistId == pricelist.Id && pi.Item.TicketType == type);
            PricelistItem pricelistItem = await _context.PricelistItems.FirstOrDefaultAsync(pi => pi.Pricelist.Active == true && pi.Item.TicketType == type);
            
            response.Data = (int) Math.Round(pricelistItem.Price);   // pomnozi sa popustom
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