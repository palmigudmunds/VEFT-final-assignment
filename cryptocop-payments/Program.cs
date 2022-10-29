using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var configSection = configuration.GetSection("RabbitMQ");

var host = configSection.GetValue<string>("Host");
var exchange = configSection.GetValue<string>("Exchange");
var queue = configSection.GetValue<string>("Queue");
var routingKey = configSection.GetValue<string>("RoutingKey");

// TODO: Setup RabbitMQ consumer
IAsyncConnectionFactory connectionFactory = new ConnectionFactory
{
    HostName = host
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue, true);
channel.QueueBind(queue, exchange, "new-order");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var routingKey = ea.RoutingKey;

    Console.WriteLine($" [x] Received '{routingKey}'");

    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // TODO: Log to log.txt
    using StreamWriter file = new("log.txt", append: true);
    file.WriteLine("\nLog: " + message);

    Console.WriteLine($" [x] Processed '{routingKey}'");
};

channel.BasicConsume(queue, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();