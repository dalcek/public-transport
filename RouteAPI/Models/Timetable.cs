using System;

namespace RouteAPI.Models
{
   public class Timetable
   {
      public int Id { get; set; }

      public int LineId { get; set; }
      public Line Line { get; set; }
      public Enums.DayType DayType { get; set; }
      public DateTime From { get; set; }
      public DateTime To { get; set; }
      public bool Active { get; set; }
   }
}