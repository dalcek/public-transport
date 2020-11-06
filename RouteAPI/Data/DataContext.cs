using System;
using Microsoft.EntityFrameworkCore;
using RouteAPI.Models;

namespace RouteAPI.Data
{
   public class DataContext : DbContext
   {
      public DbSet<Coordinate> Coordinates { get; set; }
      public DbSet<Departure> Departures { get; set; }
      public DbSet<Line> Lines { get; set; }
      public DbSet<Station> Stations { get; set; }
      public DbSet<LineStation> LineStations { get; set; }
      public DbSet<Timetable> Timetables { get; set; }
      public DataContext(DbContextOptions<DataContext> options) : base(options) { }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         modelBuilder.Entity<Coordinate>()
            .HasOne(c => c.Line)
            .WithMany(l => l.Coordinates)
            .HasForeignKey(c => c.LineId);

         modelBuilder.Entity<Departure>()
            .HasOne(d => d.Timetable)
            .WithMany(t => t.Departures)
            .HasForeignKey(d => d.TimetableId);

         modelBuilder.Entity<LineStation>()
            .HasKey(x => new { x.LineId, x.StationId });

         modelBuilder.Entity<LineStation>()
            .HasOne(ls => ls.Line)
            .WithMany(l => l.LineStations)
            .HasForeignKey(ls => ls.LineId);
         
         modelBuilder.Entity<LineStation>()
            .HasOne(ls => ls.Station)
            .WithMany(s => s.LineStations)
            .HasForeignKey(ls => ls.StationId);


         modelBuilder.Entity<Station>().HasData(
            new Station
            {
               Id = 1,
               Name = "Tri kule",
               Address = "Bul. Jovana Ducica",
               XCoordinate = 45.251052,
               YCoordinate = 19.798291,
            },
            new Station
            {
               Id = 2,
               Name = "Higijenski zavod",
               Address = "Futoska",
               XCoordinate = 45.248687,
               YCoordinate = 19.817566
            },
            new Station
            {
               Id = 3,
               Name = "Centar",
               Address = "Uspenska",
               XCoordinate = 45.254819,
               YCoordinate = 19.841785
            }
         );

         modelBuilder.Entity<Coordinate>().HasData(
            new Coordinate    
            {
               Id = 1,
               XCoordinate = 45.248883, 
               YCoordinate = 19.791697,
               LineId = 1
            },
            new Coordinate
            {
               Id = 2,
               XCoordinate = 45.253170,
               YCoordinate = 19.804232,      
               LineId = 1
            },
            new Coordinate
            {
               Id = 3,
               XCoordinate = 45.248805, 
               YCoordinate = 19.807177,
               LineId = 1
            },
            new Coordinate
            {
               Id = 4,
               XCoordinate = 45.247569, 
               YCoordinate = 19.807628,
               LineId = 1
            },
            new Coordinate
            {
               Id = 5,
               XCoordinate = 45.248597, 
               YCoordinate = 19.816578,
               LineId = 1
            },
            new Coordinate
            {
               Id = 6,
               XCoordinate = 45.249367, 
               YCoordinate = 19.822073,
               LineId = 1
            },
            new Coordinate
            {
               Id = 7,
               XCoordinate = 45.249140,  
               YCoordinate = 19.830935,
               LineId = 1
            },
            new Coordinate
            {
               Id = 8,
               XCoordinate = 45.254322, 
               YCoordinate = 19.842608,
               LineId = 1
            },
            new Coordinate
            {
               Id = 9,
               XCoordinate = 45.254877,  
               YCoordinate = 19.841879,
               LineId = 1,
            }
         );

         modelBuilder.Entity<Departure>().HasData(
            new Departure
            {
               Id = 1,
               Time = new DateTime(2020, 10, 23, 8, 45, 0),
               TimetableId = 1
            },
            new Departure
            {
               Id = 2,
               Time = new DateTime(2020, 10, 23, 9, 45, 0),
               TimetableId = 1
            },
            new Departure
            {
               Id = 3,
               Time = new DateTime(2020, 10, 23, 10, 45, 0),
               TimetableId = 1
            }
         );
         modelBuilder.Entity<Line>().HasData(
            new Line
            {
               Id = 1,
               Name = "2",
               Type = Enums.LineType.City
            }
         );

         modelBuilder.Entity<Timetable>().HasData(
            new Timetable
            {
               Id = 1,
               LineId = 1,
               DayType = Enums.DayType.Weekday,
               From = new DateTime(2020, 8, 1),
               To = new DateTime(2020, 12, 31),
               Active = true
            }
         );

         modelBuilder.Entity<LineStation>().HasData(
            new LineStation
            {
               LineId = 1,
               StationId = 1
            },
            new LineStation
            {
               LineId = 1,
               StationId = 2
            },
            new LineStation
            {
               LineId = 1,
               StationId = 3
            }
         );
      }
   }
}