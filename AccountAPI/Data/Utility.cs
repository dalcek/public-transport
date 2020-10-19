using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountAPI.Data
{
   public static class Utility
   {
      public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
      {
         using (var hmac = new System.Security.Cryptography.HMACSHA512())
         {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
         }
      }

      public static void UpdateDatabase(IApplicationBuilder app)
      {
         using (var serviceScope = app.ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope())
         {
            using (var context = serviceScope.ServiceProvider.GetService<DataContext>())
            {
               context.Database.Migrate();
            }
         }
      }
   }
}
