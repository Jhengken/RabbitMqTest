using System.Text;
using RabbitMQ.Client;

Console.WriteLine("Hello, World!");

var channel =await new RabbitMqService().Create();
// var factory = new ConnectionFactory
// {
//     UserName = "guest",
//     Password = "guest",
//     HostName = "localhost"
// };
//
// using var connection = await factory.CreateConnectionAsync();
// using var channel = await connection.CreateChannelAsync();

var queueName1 = "DirectQueue1";
var queueName2 = "DirectQueue2";
var routeKey = "testHandle";
var exchangeName = "exchangeFanout";
await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, false, false);

await channel.QueueDeclareAsync(queueName1, false, false, false);
await channel.QueueBindAsync(queueName1, exchangeName, routeKey);

await channel.QueueDeclareAsync(queueName2, false, false, false);
await channel.QueueBindAsync(queueName2, exchangeName, routeKey); 

Console.WriteLine("\nRabbitMQ連接成功,如需離開請按下Escape鍵");

var input = string.Empty;
do
{
    input = Console.ReadLine();
    var sendBytes = Encoding.UTF8.GetBytes(input);
    //發布訊息到RabbitMQ Server
    await channel.BasicPublishAsync(exchangeName, routeKey, sendBytes);

} while (Console.ReadKey().Key != ConsoleKey.Escape);

public  class RabbitMqService
{
    public  async Task<IChannel> Create()
    {
        var factory = new ConnectionFactory
        {
            UserName = "guest",
            Password = "guest",
            HostName = "localhost"
        };
        var connection = await factory.CreateConnectionAsync();
        return await connection.CreateChannelAsync();
    }
}