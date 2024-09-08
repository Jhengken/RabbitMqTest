using Lib;

Console.WriteLine("Hello, World!");

const string exchangeName = "exchangeFanout";
const string queueName1 = "DirectQueue1";
const string queueName2 = "DirectQueue2";
const string routeKey = "testHandle";

var rabbitMq =  new RabbitMqFactory(exchangeName);
await rabbitMq.InitAsync();
await rabbitMq.DeclareExchangeAsync();
await rabbitMq.DeclareQueueAsync(queueName1, routeKey);
await rabbitMq.DeclareQueueAsync(queueName2, routeKey);

Console.WriteLine("\nRabbitMQ連接成功,如需離開請按下Escape鍵");

do
{
    var message = Console.ReadLine();
    await rabbitMq.PublishAsync(message,routeKey);
} while (Console.ReadKey().Key != ConsoleKey.Escape);

