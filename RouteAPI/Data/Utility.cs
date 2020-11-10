using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RouteAPI.Data
{
   public static class Utility
   {
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

      // Calls the function multiple times until it doesn't fail
      public static void Do(Action action, TimeSpan retryInterval, int maxAttemptCount = 3)
      {
         Do<object>(() =>
         {
            Console.WriteLine("\n\n\n********************DO FUNCTION**********\n\n\n\n\n");
            action();
            return null;
         }, retryInterval, maxAttemptCount);
      }

      public static T Do<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3)
      {
         var exceptions = new List<Exception>();

         for (int attempted = 0; attempted < maxAttemptCount; attempted++)
         {
            try
            {
               if (attempted > 0)
               {
                  Thread.Sleep(retryInterval);
               }
               return action();
            }
            catch (Exception ex)
            {
               exceptions.Add(ex);
            }
         }
         throw new AggregateException(exceptions);
      }
   }
}