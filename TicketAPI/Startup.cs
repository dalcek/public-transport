using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ;
using RabbitMQ.Client;
using TicketAPI.Data;
using TicketAPI.RabbitMQClient;
using TicketAPI.Services;

namespace TicketAPI
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
         services.AddControllers();
         services.AddScoped<ITicketService, TicketService>();
         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                  .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
         services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
         
         services.AddSingleton<RabbitMQ.IRabbitMQConnection>(sp =>
         {
            var factory = new ConnectionFactory()
            {
               //HostName = Configuration["EventBus:HostName"]
               HostName = "rabbitmq"
               //UserName = "user",
               //Password = "password",
               //VirtualHost = "/",
               // HostName = "192.168.0.14",
               //Port = AmqpTcpEndpoint.UseDefaultPort
            };
            return new RabbitMQConnection(factory);
         });
         services.AddSingleton<RpcClient>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         // TODO: Write your own CORS policies, more at https://stackoverflow.com/questions/56328474/origin-http-localhost4200-has-been-blocked-by-cors-policy-in-angular7
         app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

         app.UseHttpsRedirection();

         app.UseRouting();

         app.UseAuthentication();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         Data.Utility.UpdateDatabase(app);
      }
   }
}
