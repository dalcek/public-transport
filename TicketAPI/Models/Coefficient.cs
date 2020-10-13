using static TicketAPI.Models.Enums;

namespace TicketAPI.Models
{
   public class Coefficient
   {
      public int Id { get; set; }
      public UserType UserType { get; set; }
      public double Value { get; set; }
   }
}