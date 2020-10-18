
using System;
using RabbitMQ.Client;
namespace RabbitMQ
{
    public interface IRabbitMQConnection : IDisposable
    {
      bool IsConnected { get; }
      bool TryConnect();
      IModel CreateModel();
    }
}