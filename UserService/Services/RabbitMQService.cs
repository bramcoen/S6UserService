﻿using RabbitMQ.Client;
using System.Text;

namespace UserService.Services
{
    public class RabbitMqService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqService(IConfiguration configuration)
        {
            _factory = new ConnectionFactory() { HostName = configuration.GetValue<string>("RabbitMQHostname") };
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
            }
        }
        private void SendMessage(object obj,string exchange, string routingkey)
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
