using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RouteAPI.Models;
using RouteAPI.Services;

namespace RouteAPI.Controller
{
   [Authorize(Roles = "Admin")]
   [Route("[controller]")]
   [ApiController]
   public class RouteController : ControllerBase
   {
      private readonly IRouteService _routeService;

      public RouteController(IRouteService routeService)
      {
         _routeService = routeService;
      }

      [AllowAnonymous]
      [HttpGet("test")]
      public IActionResult Test()
      {  
         //ServiceResponse<string> response = new ServiceResponse<string>();
         //response.Data = "hi from route api";
         //Newtonsoft.Json.JsonConvert.SerializeObject(response);
         //return Ok("{\"word\": \"cao route\"}");
         List<Coordinate> list = new List<Coordinate>();
         list.Add(new Coordinate{ LineId = 1, XCoordinate = 45.3213124, YCoordinate = 19.124142});
         list.Add(new Coordinate{ LineId = 1, XCoordinate = 45.35141, YCoordinate = 19.55});
         list.Add(new Coordinate{ LineId = 2, XCoordinate = 45.3213787124, YCoordinate = 19.12416542});
         list.Add(new Coordinate{ LineId = 1, XCoordinate = 45.87687, YCoordinate = 19.87});

         ServiceResponse<List<Coordinate>> tmp = new ServiceResponse<List<Coordinate>>();
         tmp.Data = list;
         string response = JsonSerializer.Serialize(tmp);
         return Ok(response);
         //return Ok(Newtonsoft.Json.JsonConvert.SerializeObject(response));
      }
      // TODO: Remove AllowAnonymous tags
      [AllowAnonymous]
      [HttpGet("getDepartures")]
      public async Task<IActionResult> GetDepartures(string dayType, int lineId)
      {  
         ServiceResponse<GetDeparturesDTO> response = await _routeService.GetDepartures(dayType, lineId);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [Authorize(Roles = "Admin")]
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
      public async Task<IActionResult> GetLineNames(string lineType)
      {
         ServiceResponse<List<LineNameDTO>> response = await _routeService.GetLineNames(lineType);
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

      [AllowAnonymous]
      [HttpGet("getLines")]
      public async Task<IActionResult> GetLines()
      {
         ServiceResponse<List<LineDTO>> response = await _routeService.GetLines();
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [AllowAnonymous]
      [HttpGet("getLineRoute")]
      public async Task<IActionResult> GetLineRoute(int id)
      {
         ServiceResponse<List<CoordinateDTO>> response = await _routeService.GetLineRoute(id);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [HttpPost("addLine")]
      public async Task<IActionResult> AddLine(AddLineDTO request)
      {
         ServiceResponse<string> response = await _routeService.AddLine(request);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [HttpPut("editLine")]
      public async Task<IActionResult> UpdateLine(LineDTO request)
      {
         ServiceResponse<List<LineDTO>> response = await _routeService.UpdateLine(request);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }

      [HttpDelete("deleteLine")]
      public async Task<IActionResult> DeleteLine(int id)
      {
         ServiceResponse<int> response = await _routeService.DeleteLine(id);
         if (!response.Success)
         {
            return BadRequest(response);
         }
         return Ok(response);
      }
   }
}