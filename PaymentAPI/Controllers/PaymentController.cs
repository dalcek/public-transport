using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Models;
using PaymentAPI.Services;

namespace PaymentAPI.Controllers
{
   [Authorize]
   [Route("[controller]")]
   [ApiController]
   public class PaymentController : ControllerBase
   {
      private readonly IPaymentService _paymentService;
      private readonly IMapper _mapper;

      public PaymentController(IPaymentService paymentService, IMapper mapper)
      {
         _paymentService = paymentService;
         _mapper = mapper;
      }

      [AllowAnonymous]
      [HttpPost("addPayment")]
      public async Task<IActionResult> AddPayment(PaymentDTO request)
      {
         Payment payment = _mapper.Map<Payment>(request);
         ServiceResponse<bool> response = await _paymentService.AddPayment(payment);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }
   }
}