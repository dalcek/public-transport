using System.Threading.Tasks;
using PaymentAPI.Models;

namespace PaymentAPI.Services
{
   public interface IPaymentService
   {
      Task<ServiceResponse<bool>> AddPayment(Payment payment);
   }
}