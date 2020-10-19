namespace PaymentAPI.Models
{
   public class Payment
   {
      public int Id { get; set; }
      public string TransactionId { get; set; }
      public string PayerId { get; set; }
      public string PayerEmail { get; set; }
      public int TicketId { get; set; }      
   }
}