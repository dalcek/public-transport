namespace TicketAPI.Models
{
   public class PricelistItem
   {
      public int Id { get; set; }
      public double Price { get; set; }
      public int PricelistId { get; set; }
      public Pricelist Pricelist { get; set; }
      public int ItemId { get; set; }
      public Item Item { get; set; }
      //mozda ne treba, stavljam zbog seeda
      //public int TicketId { get; set; }
      //public Ticket Ticket { get; set; }
   }
}