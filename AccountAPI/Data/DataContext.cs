using AccountAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // Data seeding method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(user => user.Role).HasDefaultValue("AppUser");

            modelBuilder.Entity<User>()
            .Property(user => user.UserStatus).HasDefaultValue(Models.Enums.UserStatus.InProcess);

            Utility.CreatePasswordHash("Admin123!", out byte[] passwordHash, out byte[] passwordSalt);

            modelBuilder.Entity<User>().HasData(
                new User
                { 
                    Id = 1,
                    Email = "admin@gmail.com",
                    Name = "Joe",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 4, 1),
                    UserStatus = Enums.UserStatus.Accepted,
                    Role = "Admin",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }    
            );

            Utility.CreatePasswordHash("Controller123!", out passwordHash, out passwordSalt);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Email = "controller@gmail.com",
                    Name = "Jane",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1993, 7, 23),
                    UserStatus = Enums.UserStatus.Accepted,
                    Role = "Controller",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }
            );

            Utility.CreatePasswordHash("Appuser123!", out passwordHash, out passwordSalt);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 3,
                    Email = "appuser@gmail.com",
                    Name = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1996, 9, 3),
                    UserStatus = Enums.UserStatus.Accepted,
                    UserType = Enums.UserType.RegularUser,
                    Role = "AppUser",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }
            );

            Utility.CreatePasswordHash("Nikola123!", out passwordHash, out passwordSalt);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 4,
                    Email = "nikola@gmail.com",
                    Name = "Nikola",
                    LastName = "Dragas",
                    DateOfBirth = new DateTime(1996, 9, 3),
                    UserStatus = Enums.UserStatus.Accepted,
                    UserType = Enums.UserType.Student,
                    Role = "AppUser",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                }
            );
        }
    }
}
