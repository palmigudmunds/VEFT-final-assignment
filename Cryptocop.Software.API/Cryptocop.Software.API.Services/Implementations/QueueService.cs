using Cryptocop.Software.API.Services.Interfaces;
using FloatAway.Gateway.Services.Helpers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class QueueService : IQueueService, IDisposable
    {
        private readonly string _exchangeName;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public QueueService(IConfiguration configuration)
        {
            var configSection = configuration.GetSection("RabbitMQ");

            _exchangeName = configSection.GetValue<string>("Exchange");

            IAsyncConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = configSection.GetValue<string>("Host")
            };

            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void PublishMessage(string routingKey, object body)
        {
            _channel.BasicPublish(_exchangeName, routingKey, body: Encoding.UTF8.GetBytes(JsonSerializerHelper.SerializeWithCamelCasing(body)));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _channel.Dispose();
            _connection.Dispose();
        }
    }
}