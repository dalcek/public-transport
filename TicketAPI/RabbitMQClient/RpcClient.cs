using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TicketAPI.RabbitMQClient
{
   public class RpcClient
   {
      private const string QUEUE_NAME = "rpc_queue";
      //private readonly IConnection _connection;
      private readonly IRabbitMQConnection _connection;
      private readonly IModel channel;
      private readonly string replyQueueName;
      private readonly EventingBasicConsumer consumer;
      private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper =
         new ConcurrentDictionary<string, TaskCompletionSource<string>>();

      public RpcClient(IRabbitMQConnection connection)
      {
        _connection = connection;
        channel = connection.CreateModel();
        replyQueueName = channel.QueueDeclare().QueueName;
        consumer = new EventingBasicConsumer(channel);
        Console.WriteLine("RPC Client pre consume");
        consumer.Received += (model, ea) =>
        {
            if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<string> tcs))
            {
               return;
            }
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
        };
        Console.WriteLine("RPC Client odradio1");
      }
      public Task<string> CallAsync(string message, CancellationToken cancellationToken = default(CancellationToken))
      {
         IBasicProperties props = channel.CreateBasicProperties();
         var correlationId = Guid.NewGuid().ToString();
         props.CorrelationId = correlationId;
         props.ReplyTo = replyQueueName;
         var messageBytes = Encoding.UTF8.GetBytes(message);
         var tcs = new TaskCompletionSource<string>();
         callbackMapper.TryAdd(correlationId, tcs);
         
         channel.BasicPublish(
            exchange: "",
            routingKey: QUEUE_NAME,
            basicProperties: props,
            body: messageBytes);
         
         channel.BasicConsume(
            consumer: consumer,
            queue: replyQueueName,
            autoAck: true);
         
         cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out var tmp));
         return tcs.Task;
      }
   }
}