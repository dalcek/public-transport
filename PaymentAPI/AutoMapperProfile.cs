using AutoMapper;
using PaymentAPI.Models;

namespace PaymentAPI
{
   public class AutoMapperProfile : Profile
   {
      public AutoMapperProfile()
      {
         CreateMap<PaymentDTO, Payment>();
      }
   }
}