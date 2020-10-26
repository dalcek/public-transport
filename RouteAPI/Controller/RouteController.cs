using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteAPI.Models;
using RouteAPI.Services;

namespace RouteAPI.Controller
{
   [Authorize]
   [Route("[controller]")]
   [ApiController]
   public class RouteController : ControllerBase
   {
      private readonly IRouteService _routeService;

      public RouteController(IRouteService routeService)
      {
         _routeService = routeService;
      }
      // TODO: Remove AllowAnonymous tags
      [AllowAnonymous]
      [HttpGet("getDepartures")]
      public async Task<IActionResult> GetDepartures(string dayType, string lineType, int lineId)
      {  //Remove the lineType argument
         ServiceResponse<GetDeparturesDTO> response = await _routeService.GetDepartures(dayType, lineId);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpPost("addDeparture")]
      public async Task<IActionResult> AddDeparture(AddDepartureDTO request)
      {
         ServiceResponse<GetDeparturesDTO> response = await _routeService.AddDeparture(request);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpPut("editDeparture")]
      public async Task<IActionResult> UpdateDeparture(AddDepartureDTO request)
      {
         ServiceResponse<GetDeparturesDTO> response = await _routeService.UpdateDeparture(request);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpDelete("deleteDeparture")]
      public async Task<IActionResult> DeleteDeparture(int id)
      {
         ServiceResponse<int> response = await _routeService.DeleteDeparture(id);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpGet("getLineNames")]
      public async Task<IActionResult> GetLineNames(string dayType, string lineType)
      {
         ServiceResponse<List<LineNameDTO>> response = await _routeService.GetLineNames(dayType, lineType);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }
   }
}