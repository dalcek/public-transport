using System;
using System.Threading.Tasks;
using AutoMapper;
using PaymentAPI.Data;
using PaymentAPI.Models;

namespace PaymentAPI.Services
{
   public class PaymentService : IPaymentService
   {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      public PaymentService(DataContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }

      public async Task<ServiceResponse<bool>> AddPayment(Payment payment)
      {
         ServiceResponse<bool> response = new ServiceResponse<bool>();

         try
         {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            response.Data = true;
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