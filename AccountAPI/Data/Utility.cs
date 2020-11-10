using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
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

      public static void SendEmail(string to, string subj, string body)
      {
         try
         {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("nikoladevnalog@gmail.com");
            mail.To.Add(to);
            mail.To.Add("n.dragas9@gmail.com");
            mail.Subject = subj;
            mail.Body = body;

            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.EnableSsl = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("nikoladevnalog@gmail.com", "NikolaDev996");

            SmtpServer.Send(mail);
         }
         catch(Exception e)
         {
            Console.WriteLine("Sending email failed. Error message: " + e.Message);
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
