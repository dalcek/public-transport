using System;

namespace RouteAPI.Models
{
   public class Departure
   {
      public int Id { get; set; }
      public DateTime Time { get; set; }
      public int TimetableId { get; set; }
      public Timetable Timetable { get; set; }
   }
}