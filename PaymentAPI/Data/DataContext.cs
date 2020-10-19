using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;

namespace PaymentAPI.Data
{
   public class DataContext : DbContext
   {
      public DbSet<Payment> Payments { get; set; }

      public DataContext(DbContextOptions<DataContext> options) : base(options) { }

      // For database references and seeding
      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         
      }
   }
}