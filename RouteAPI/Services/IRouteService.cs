using System.Collections.Generic;
using System.Threading.Tasks;
using RouteAPI.Models;

namespace RouteAPI.Services
{
   public interface IRouteService
   {
      Task<ServiceResponse<GetDeparturesDTO>> GetDepartures(string dayType, int lineId);
      Task<ServiceResponse<GetDeparturesDTO>> AddDeparture(AddDepartureDTO newDeparture);
      Task<ServiceResponse<GetDeparturesDTO>> UpdateDeparture(AddDepartureDTO newDeparture);
      Task<ServiceResponse<int>> DeleteDeparture(int id);
      Task<ServiceResponse<List<LineNameDTO>>> GetLineNames(string dayType, string lineType);
      Task<ServiceResponse<List<Station>>> GetStations();
      Task<ServiceResponse<List<StationDTO>>> GetStationNames();
      Task<ServiceResponse<List<Station>>> AddStation(AddStationDTO station);
      Task<ServiceResponse<List<Station>>> UpdateStation(Station newStation);
      Task<ServiceResponse<List<Station>>> DeleteStation(int id);
   }
}