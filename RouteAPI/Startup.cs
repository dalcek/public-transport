using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
using RouteAPI.Data;
using RouteAPI.RabbitMQServer;
using RouteAPI.Services;

namespace RouteAPI
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
         services.AddAutoMapper(typeof(Startup));
         services.AddScoped<IRouteService, RouteService>();
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

         services.AddSingleton<IRabbitMQConnection>(sp =>
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

            services.AddSingleton<RpcServer>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

         app.UseHttpsRedirection();

         app.UseRouting();

         app.UseAuthentication();

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         Data.Utility.Do(() => Data.Utility.UpdateDatabase(app), TimeSpan.FromSeconds(40), 5);
         Data.Utility.Do(() => app.UseRabbitListener(), TimeSpan.FromSeconds(40), 5);

         //Data.Utility.UpdateDatabase(app);
         //app.UseRabbitListener();
      }
   }
}
