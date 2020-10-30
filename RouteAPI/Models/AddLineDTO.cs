using System.Collections.Generic;

namespace RouteAPI.Models
{
   public class AddLineDTO
   {
      public string Name { get; set; }
      public string Type { get; set; }
      public List<CoordinateDTO> Coordinates { get; set; }
   }
}