using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory
{
    UserName = "guest",
    Password = "guest",
    HostName = "localhost"
};

var queueName = "DirectQueue1";

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

//channel.QueueBind
var consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicQosAsync(0, 5, true);  // 可以一次拿五個
//接收到消息事件 consumer.IsRunning
consumer.Received += async (ch, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

    Console.WriteLine($"Queue:{queueName}收到資料： {message}");
    await Task.Delay(3000);
    await channel.BasicAckAsync(ea.DeliveryTag, false);
};

await channel.BasicConsumeAsync(queueName, false, consumer); 
Console.WriteLine("接收訊息");
Console.ReadKey();