using System.Collections.Generic;
using static RouteAPI.Models.Enums;

namespace RouteAPI.Models
{
   public class Line
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public LineType Type { get; set; }
      public List<Coordinate> Coordinates { get; set; }
      public List<LineStation> LineStations { get; set; }      
   }
}