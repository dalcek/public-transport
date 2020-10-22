namespace RouteAPI.Models
{
   public class LineStation
   {
      public int LineId { get; set; }
      public Line Line { get; set; }
      public int StationId { get; set; }
      public Station Station { get; set; }
   }
}