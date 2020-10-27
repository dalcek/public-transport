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

      [AllowAnonymous]
      [HttpGet("getStations")]
      public async Task<IActionResult> GetStations()
      {
         ServiceResponse<List<Station>> response = await _routeService.GetStations();
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpGet("getStationNames")]
      public async Task<IActionResult> GetStationNames()
      {
         ServiceResponse<List<StationDTO>> response = await _routeService.GetStationNames();
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpPost("addStation")]
      public async Task<IActionResult> AddStation(AddStationDTO request)
      {
         ServiceResponse<List<Station>> response = await _routeService.AddStation(request);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }
      [AllowAnonymous]
      [HttpPut("editStation")]
      public async Task<IActionResult> EditStation(Station request)
      {
         ServiceResponse<List<Station>> response = await _routeService.UpdateStation(request);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpDelete("deleteStation")]
      public async Task<IActionResult> DeleteStation(int id)
      {
         ServiceResponse<List<Station>> response = await _routeService.DeleteStation(id);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }
   }
}