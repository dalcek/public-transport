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
      Task<ServiceResponse<AddedTicketDTO>> CreateTicket(string ticketType, int ticketPrice, string email);
      Task<ServiceResponse<int>> DeleteTicket(int id);
   }
}