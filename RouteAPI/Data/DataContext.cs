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
            },
            new Coordinate
            {
               Id = 10,
               XCoordinate = 45.236991, 
               YCoordinate = 19.826449,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 11,
               XCoordinate = 45.238233,   
               YCoordinate = 19.830771,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 12,
               XCoordinate = 45.239185,   
               YCoordinate = 19.834213,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 13,
               XCoordinate = 45.239605,   
               YCoordinate = 19.835805,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 14,
               XCoordinate = 45.241640,   
               YCoordinate = 19.842816,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 15,
               XCoordinate = 45.244140,    
               YCoordinate = 19.841381,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 16,
               XCoordinate = 45.246419,    
               YCoordinate = 19.840148,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 17,
               XCoordinate = 45.247974,    
               YCoordinate = 19.839265,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 18,
               XCoordinate = 45.247740,    
               YCoordinate = 19.836482,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 19,
               XCoordinate = 45.248658,    
               YCoordinate = 19.833655,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 20,
               XCoordinate = 45.249700,    
               YCoordinate = 19.832616,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 21,
               XCoordinate = 45.249218,    
               YCoordinate = 19.830916,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 22,
               XCoordinate = 45.249295,    
               YCoordinate = 19.824555,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 23,
               XCoordinate = 45.248145,    
               YCoordinate = 19.824997,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 24,
               XCoordinate = 45.245548,    
               YCoordinate = 19.825107,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 25,
               XCoordinate = 45.243386,    
               YCoordinate = 19.825240,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 26,
               XCoordinate = 45.239845,    
               YCoordinate = 19.825331,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 27,
               XCoordinate = 45.238532,    
               YCoordinate = 19.825835,
               LineId = 2,
            },
            new Coordinate
            {
               Id = 28,
               XCoordinate = 45.237127,    
               YCoordinate = 19.826510,
               LineId = 2,
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

         modelBuilder.Entity<Line>().HasData(
            new Line
            {
               Id = 2,
               Name = "3",
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