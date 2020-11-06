using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RouteAPI.Data;
using RouteAPI.Models;
using RouteAPI.Services;

namespace RouteAPI.RabbitMQServer
{
   public class RpcServer
   {
      private readonly IRabbitMQConnection _connection;
      private readonly IRouteService _routeService;
      public RpcServer(IRabbitMQConnection connection, IRouteService routeService)
      {
         _connection = connection ?? throw new ArgumentNullException(nameof(connection));
         _routeService = routeService;
      }

      public void Consume()
      {
         var channel = _connection.CreateModel();
         channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
         channel.BasicQos(0, 1, false);
         var consumer = new EventingBasicConsumer(channel);
         channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);
         
         consumer.Received += (model, ea) =>
         {
            string response = null;

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
               var message = Encoding.UTF8.GetString(body);
               Console.WriteLine(message);
               //string res = "caos rabbit";
               var tmp = _routeService.GetCoordinates();
               response = JsonSerializer.Serialize(tmp);
            }
            catch (Exception e)
            {
               Console.WriteLine("Error at RPC Server: " + e.Message);
               response = "";
            }
            finally
            {
               var responseBytes = Encoding.UTF8.GetBytes(response);
               channel.BasicPublish(exchange: "", routingKey: props.ReplyTo, basicProperties: replyProps, body: responseBytes);
               channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
         };
      }
      public void Disconnect()
      {
         _connection.Dispose();
      }
   }
}