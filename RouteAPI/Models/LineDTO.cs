using System.Collections.Generic;

namespace RouteAPI.Models
{
   public class LineDTO
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Type { get; set; }
      public List<int> StationIds { get; set;}
   }
}