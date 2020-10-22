using System;

namespace TicketAPI.Models
{
   public class PricelistDTO
   {
      public string From { get; set; }
      public string To { get; set; }
      public int HourPrice { get; set; }
      public int DayPrice { get; set; }
      public int MonthPrice { get; set; }
      public int YearPrice { get; set; }

   }
}