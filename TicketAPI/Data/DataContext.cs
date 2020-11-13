using Microsoft.EntityFrameworkCore;
using TicketAPI.Models;

namespace TicketAPI.Data
{
   public class DataContext : DbContext
   {
      public DbSet<Item> Items { get; set; }
      public DbSet<Pricelist> Pricelists { get; set; }
      public DbSet<PricelistItem> PricelistItems { get; set; }
      public DbSet<Ticket> Tickets { get; set; }
      public DbSet<Coefficient> Coefficients { get; set; }
      public DataContext(DbContextOptions<DataContext> options) : base(options) { }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Ticket>().HasKey(
            t => new { t.Id }
         );

         modelBuilder.Entity<Coefficient>().HasData(
            new Coefficient
            {
               Id = 1,
               UserType = Enums.UserType.RegulerUser,
               Value = 1
            },
            new Coefficient
            {
               Id = 2,
               UserType = Enums.UserType.Student,
               Value = 0.8
            },
            new Coefficient
            {
               Id = 3,
               UserType = Enums.UserType.Retired,
               Value = 0.85
            }
         );
         
         modelBuilder.Entity<Item>().HasData(
            new Item
            {
               Id = 1,
               TicketType = Enums.TicketType.HourTicket
            },
            new Item
            {
               Id = 2,
               TicketType = Enums.TicketType.DayTicket
            },
            new Item
            {
               Id = 3,
               TicketType = Enums.TicketType.MonthTicket
            },
            new Item
            {
               Id = 4,
               TicketType = Enums.TicketType.YearTicket
            }
         );
         
         modelBuilder.Entity<Pricelist>().HasData(
            new Pricelist
            {
               Id = 1,
               From = new System.DateTime(2020, 4, 1),
               To = new System.DateTime(2020, 12, 31),
               Active = true
            }
         );
         
         modelBuilder.Entity<PricelistItem>().HasData(
            new PricelistItem
            {
               Id = 1,
               Price = 70,
               PricelistId = 1,
               ItemId = 1,
            },
            new PricelistItem
            {
               Id = 2,
               Price = 250,
               PricelistId = 1,
               ItemId = 2,
            },
            new PricelistItem
            {
               Id = 3,
               Price = 1600,
               PricelistId = 1,
               ItemId = 3,
            },
            new PricelistItem
            {
               Id = 4,
               Price = 12000,
               PricelistId = 1,
               ItemId = 4,
            }
         );
         
         modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
               Id = 1,
               IssueTime = new System.DateTime(2020, 8, 12, 8, 22, 12),
               PricelistItemId = 1,
               Valid = true,
               Price = 70,
               UserId = 3
            },
            new Ticket
            {
               Id = 2,
               IssueTime = new System.DateTime(2020, 6, 5, 16, 13, 56),
               PricelistItemId = 2,
               Valid = true,
               Price = 250,
               UserId = 3
            },
             new Ticket
            {
               Id = 3,
               IssueTime = new System.DateTime(2020, 11, 12, 10, 13, 0),
               PricelistItemId = 4,
               Valid = true,
               Price = 10000,
               UserId = 3
            }
         );
      }
   }
}