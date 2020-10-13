using System;

namespace TicketAPI.Models
{
   public class Ticket
   {
      public int Id { get; set; }
      public DateTime IssueTime { get; set; }
      public int PricelistItemId { get; set; }
      public PricelistItem PricelistItem { get; set; }
      public bool Valid { get; set; } = true;
      public double Price { get; set; }
      public int UserId { get; set; }
      //public User User { get; set; }   
   }
}