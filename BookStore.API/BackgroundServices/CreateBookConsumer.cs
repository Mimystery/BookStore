using BookStore.API.Contracts;
using BookStore.Application.Services;
using BookStore.Core.Models;
using BookStore.Core.Options;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookStore.BackgroundServices
{
    public class CreateBookConsumer : BackgroundService
    {
        private readonly RabbitMqOptions _rabbitMqOptions;
        private readonly IChannel _channel;
        private readonly IBooksService _booksService;
        private readonly IServiceProvider _serviceProvider;

        public CreateBookConsumer(IOptions<RabbitMqOptions> options, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _rabbitMqOptions = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqOptions.HostName,
                Port = _rabbitMqOptions.Port,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password,
                VirtualHost = _rabbitMqOptions.VirtualHost
            };
            var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                var bookRequest = JsonSerializer.Deserialize<BooksRequest>(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                });
                using var scope = _serviceProvider.CreateScope();
                var booksService = scope.ServiceProvider.GetRequiredService<IBooksService>();
                var (book, error) = Book.Create(Guid.NewGuid(), bookRequest.Title, bookRequest.Description, bookRequest.Price);

                await booksService.CreateBook(book);

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
            };

            await _channel.BasicConsumeAsync(_rabbitMqOptions.QueueName, autoAck: false, consumer,
                 cancellationToken: stoppingToken);
        }
    }
}
