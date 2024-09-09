using Lib;

Console.WriteLine("Hello, This is Consumer2");

const string queueName = "DirectQueue2";

var rabbitMq = new RabbitMqFactory(queueName);
await rabbitMq.InitAsync();
await rabbitMq.DeclareExchangeAsync();
await rabbitMq.Consume(queueName);

Console.WriteLine("接收訊息");
Console.ReadKey();