using Newtonsoft.Json;
using Optsol.EventDriven.Components.Core.Domain;
using RabbitMQ.Client;
using System.Text;

namespace Optsol.EventDriven.Components.Driven.Infra.Notification;

public class MessageBus : IMessageBus
{
    private readonly ServiceBusSettings _settings;

    public MessageBus(ServiceBusSettings settings)
    {
        _settings = settings;
    }

    public Task Publish<T>(IEnumerable<T> events, string routingKey)
    {
        var factory = CreateConnectionFactory();
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _settings.Exchange,
                                    type: ExchangeType.Topic);

            foreach (var @event in events)
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

                channel.BasicPublish(exchange: _settings.Exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }
        return Task.CompletedTask;
    }

    private ConnectionFactory CreateConnectionFactory()
    {
        if(string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            return new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password,
                Port = _settings.Port ?? 5672
            };
        }

        return new ConnectionFactory() { HostName = _settings.ConnectionString };
    }
}