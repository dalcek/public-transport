﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketAPI.Data;

namespace TicketAPI.Data
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
   }
}