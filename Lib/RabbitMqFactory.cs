using System.Text;
using RabbitMQ.Client;

namespace Lib;

public class RabbitMqFactory(string exchangeName)
{
    private IConnection? _connection;
    private IChannel? _channel;

    public async Task Init()
    {
        var factory = new ConnectionFactory
        {
            UserName = "guest",
            Password = "guest",
            HostName = "localhost"
        };
        _connection = await factory.CreateConnectionAsync();
        _channel  = await _connection.CreateChannelAsync();
    }

    public async Task DeclareQueueAsync(string queueName1, string routeKey)
    {
        await _channel!.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, false, false);

        await _channel.QueueDeclareAsync(queueName1, false, false, false);
        await _channel.QueueBindAsync(queueName1, exchangeName, routeKey);
    }

    public async Task PublishAsync(string? msg, string routeKey)
    {
        if (string.IsNullOrWhiteSpace(msg))
        {
            throw new ArgumentNullException(msg);
        }
        var sendBytes = Encoding.UTF8.GetBytes(msg);
        await _channel!.BasicPublishAsync(exchangeName, routeKey, sendBytes);
    }
}