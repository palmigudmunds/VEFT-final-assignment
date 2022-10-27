using System.Text.Json.Serialization;
using Cryptocop.Software.API.Services.Implementations;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cryptocop.Software.API.Repositories;
using Cryptocop.Software.API.Repositories.Implementations;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cryptocop.Software.API.Middlewares;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using FloatAway.Gateway.Services.Helpers;
using Cryptocop.Software.API.Models.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Register services to the container.
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<IExchangeService, ExchangeService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();

builder.Services.AddHttpClient<ICryptoCurrencyService, CryptoCurrencyService>(client => {
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CryptoApiBaseUrl"));
});

var jwtConfig = builder.Configuration.GetSection("JwtConfig");
builder.Services.AddTransient<ITokenService>((c) => new TokenService(
    jwtConfig.GetValue<string>("secret"),
    jwtConfig.GetValue<string>("expirationInMinutes"),
    jwtConfig.GetValue<string>("issuer"),
    jwtConfig.GetValue<string>("audience")
));

// Register repositories to the container.
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITokenRepository, TokenRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>();

builder.Services.AddDbContext<CryptocopDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("CryptocopConnectionString"), b => b.MigrationsAssembly("Cryptocop.Software.API"));
});

builder.Services.AddAuthentication(config => {

    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtTokenAuthentication(builder.Configuration);

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
});
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// RabbitMQ connections
// var configuration = new ConfigurationBuilder()
//     .AddJsonFile("appsettings.json")
//     .Build();

// var configSection = configuration.GetSection("RabbitMQ");

// var host = configSection.GetValue<string>("Host");
// var exchange = configSection.GetValue<string>("Exchange");
// var queue = configSection.GetValue<string>("Queue");
// var routingKey = configSection.GetValue<string>("RoutingKey");

// IAsyncConnectionFactory connectionFactory = new ConnectionFactory
// {
//     HostName = host
// };

// using var connection = connectionFactory.CreateConnection();
// using var channel = connection.CreateModel();

// channel.QueueDeclare(queue, true);
// channel.QueueBind(queue, exchange, "create-order");

// Console.WriteLine(" [*] Waiting for new orders. To exit press CTRL+C");

// var consumer = new EventingBasicConsumer(channel);

// consumer.Received += (model, ea) =>
// {
//     var routingKey = ea.RoutingKey;

//     Console.WriteLine($" [x] Received '{routingKey}'");

//     var body = ea.Body.ToArray();
//     var message = Encoding.UTF8.GetString(body);

//     var inputModel = JsonSerializerHelper.DeserializeWithCamelCasing<OrderDto>(message);
//     if (inputModel == null) { throw new Exception("The order cannot be null."); }

//     Console.WriteLine($" [x] Processed '{routingKey}'");
// };
// channel.BasicConsume(queue, autoAck: true, consumer: consumer);

// Console.WriteLine(" Press [enter] to exit.");
// Console.ReadLine();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();