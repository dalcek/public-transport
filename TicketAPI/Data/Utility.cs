using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
   }
}
