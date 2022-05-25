﻿using RabbitMQ.Client;
using System.Text;

namespace UserService.Services
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQService(IConfiguration configuration)
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq.default.svc.cluster.local", UserName = configuration.Get["RabbitMQUsername"], Password = configuration.Get["RabbitMQPassword"] };
        }
        public void SendMessage(object obj, string exchange, string routingkey)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var json = System.Text.Json.JsonSerializer.Serialize(obj);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: exchange,
                                     routingKey: routingkey,
                                     basicProperties: null,
                                     body: body);
                // _logger.LogInformation(" [x] Sent {0}", message);
            }
        }
    }
}

