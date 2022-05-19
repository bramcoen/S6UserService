/*using DataInterface;
using Models;
using MongoDBRepository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace UserService.BackgroundWorkers
{
    public class NewTokenWorker : BackgroundService
    {
        IUserRepository _userRepository;
        ILogger _logger;
        ConnectionFactory _connectionFactory;
        IConnection _connection;
        IModel _channel;
        IConfiguration _configuration;
        public NewTokenWorker(ILogger<NewTokenWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _userRepository = new UserRepository(configuration);
            _configuration = configuration;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Json.JsonSerializer.Deserialize<Token>(body, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                if (message == null) return;
                if (message!.Email != null)
                {
                    if (message != null) await _userRepository.RegisterProviderAccount(message);
                }
                if (message!.Token != null)
                {
                    message.Token.ProviderAccountId = message.ProviderAccountId;
                    await _userRepository.RegisterToken(message.Token);
                }
               // if (message.Email != null)
               // {
               //     await _userRepository.RegisterUser(message.Email);
               // }
                _logger.LogInformation(" [x] Received {0}", message!.ProviderAccountId);
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "user/newToken",
                                 autoAck: false,
                                 consumer: consumer);

            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHostname"],
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "user/newToken",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
            _channel.BasicQos(0, 30, false);
            _logger.LogInformation($"Queue [hello] is waiting for messages.");

            return base.StartAsync(cancellationToken);
        }
    }
}
*/