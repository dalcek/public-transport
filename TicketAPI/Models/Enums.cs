namespace TicketAPI.Models
{
   public class Enums
   {
      public enum TicketType
      {
         HourTicket = 1,
         DayTicket,
         MonthTicket,
         YearTicket
      }

      public enum UserType
      {
         RegulerUser = 1,
         Student,
         Retired
      }
   }
}