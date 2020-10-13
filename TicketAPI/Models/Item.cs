using System.Collections.Generic;
using static TicketAPI.Models.Enums;

namespace TicketAPI.Models
{
   public class Item
   {
      public int Id { get; set; }
      public TicketType TicketType { get; set; }
      //public List<PricelistItem> PricelistItems { get; set; }  //mozda ne treba
   }
}