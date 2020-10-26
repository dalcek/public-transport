using System.Collections.Generic;

namespace RouteAPI.Models
{
   public class GetDeparturesDTO
   {
      public List<DepartureDTO> Departures { get; set; }
      public int TimetableId { get; set; }
   }
}