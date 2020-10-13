using System.Collections.Generic;
using System.Threading.Tasks;
using TicketAPI.Models;

namespace TicketAPI.Services
{
   public interface ITicketService
   {
      Task<ServiceResponse<List<double>>> GetAllPrices();
      Task<ServiceResponse<int>> GetPrice(string ticketType);
      Task<ServiceResponse<List<Coefficient>>> GetCoefficients();
      Task<ServiceResponse<AddedTicketDTO>> AddTicket(string ticketType);
   }
}