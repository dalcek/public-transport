using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AccountAPI.Models;
using AccountAPI.Services;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AccountAPI.RabbitMQServer
{
   public class RpcServer
   {
      private readonly IRabbitMQConnection _connection;
      private readonly IAccountService _accountService;
      public RpcServer(IRabbitMQConnection connection, IAccountService accounetService)
      {
         _connection = connection ?? throw new ArgumentNullException(nameof(connection));
         _accountService = accounetService;
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
               int id = int.Parse(message);

               ServiceResponse<Enums.UserType> res = _accountService.GetUserType(id);
               response = JsonSerializer.Serialize(res);
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