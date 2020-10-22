using System.Collections.Generic;
using static RouteAPI.Models.Enums;

namespace RouteAPI.Models
{
   public class Line
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public LineType Type { get; set; }
      // TODO: Add fluent expression for this
      public List<Coordinate> Coordinates { get; set; }      
   }
}