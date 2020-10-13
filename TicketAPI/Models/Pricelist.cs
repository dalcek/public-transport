using System;
using System.Collections.Generic;

namespace TicketAPI.Models
{
   public class Pricelist
   {
      public int Id { get; set; }
      public DateTime From { get; set; }
      public DateTime To { get; set; }
      public bool Active { get; set; }
      //public List<PricelistItem> PricelistItems { get; set; }  //mozda ne treba
   }
}