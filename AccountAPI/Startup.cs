using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.Data;
using AccountAPI.RabbitMQServer;
using AccountAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ;
using RabbitMQ.Client;

namespace AccountAPI
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
            services.AddScoped<IAccountService, AccountService>();
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

            services.Configure<FormOptions>(o => 
            {
               o.ValueLengthLimit = int.MaxValue;
               o.MultipartBodyLengthLimit = int.MaxValue;
               o.MemoryBufferThreshold = int.MaxValue;
            });
            /*
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
            */
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

            // If running without Docker, create folder AccountAPI/Resources/Images
            Directory.CreateDirectory("/app/Resources/Images");
            app.UseStaticFiles(new StaticFileOptions 
            {
               FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
               RequestPath = new PathString("/Resources")
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Data.Utility.UpdateDatabase(app);
            //Initilize Rabbit Listener in ApplicationBuilderExtentions
            //app.UseRabbitListener();
        }
    }
}
