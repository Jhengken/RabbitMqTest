using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Lib;

public class RabbitMqFactory(string exchangeName)
{
    private readonly ConnectionFactory _factory = new()
    {
        UserName = "guest",
        Password = "guest",
        HostName = "localhost"
    };

    private IChannel? _channel;
    private IConnection? _connection;

    private bool ConnectionIsOpen => _connection is not null && _connection.IsOpen;

    public async Task Consume(string queueName)
    {
        if (_channel == null)
        {
            throw new NullReferenceException($"RabbitMQ channel is null, should use {nameof(InitAsync)}");
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        await _channel.BasicQosAsync(0, 5, false); // 可以一次拿五個

        //接收到消息事件 consumer.IsRunning
        consumer.Received += async (_, ea) =>
        {
            var guid = Guid.NewGuid().ToString()[..5];

            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"[{guid}] Queue:{queueName}, 收到資料： {message}");
            await Task.Delay(1000);
            Console.WriteLine($"[{guid}] Queue:{queueName}, 結束");

            await _channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await _channel.BasicConsumeAsync(queueName, false, consumer);
    }

    public Task DeclareExchangeAsync()
    {
        return _channel!.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, false, false);
    }

    public async Task DeclareQueueAsync(string queueName, string routeKey)
    {
        await _channel!.QueueDeclareAsync(queueName, false, false, false);
        await _channel.QueueBindAsync(queueName, exchangeName, routeKey);
    }

    public void Dispose()
    {
        if (ConnectionIsOpen)
        {
            _connection!.Dispose();
        }
    }

    public async Task InitAsync()
    {
        await Connection();
        _channel = await _connection!.CreateChannelAsync();
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

    private async Task Connection()
    {
        try
        {
            _connection = await _factory.CreateConnectionAsync();
            Console.WriteLine("RabbitMq connection success");
        }
        catch (Exception e)
        {
            Console.WriteLine($"RabbitMq connection fail, Exception Message:{e.Message}");
        }

        if (ConnectionIsOpen)
        {
            _connection!.ConnectionShutdown += async (_, _) => await OnConnectionShutdown();
        }
    }

    private async Task OnConnectionShutdown()
    {
        Console.WriteLine("A RabbitMQ connection is on shutdown. Trying to re-connect...");
        await Connection();
    }
}
