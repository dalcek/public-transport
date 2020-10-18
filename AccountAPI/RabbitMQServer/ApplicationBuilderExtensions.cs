using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccountAPI.RabbitMQServer
{
   public static class ApplicationBuilderExtensions
   {
      public static RpcServer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RpcServer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            Console.WriteLine("Listener");
            Console.WriteLine(Listener.ToString());

            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
   }
}