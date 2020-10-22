namespace RouteAPI.Models
{
   public class Coordinate
   {
      public int Id { get; set; }
      public double XCoordinate { get; set; }
      public double YCoordinate { get; set; }
      public int LineId { get; set; }
      public Line Line { get; set; }
   }
}