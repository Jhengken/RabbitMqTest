using Lib;

Console.WriteLine("Hello, This is Consumer2");

var rabbitMq = new RabbitMqFactory($"exchangeName_{ExchangeTypes.FanOut}");
await rabbitMq.RegisterConsume(ExchangeTypes.FanOut, $"queueName2_{ExchangeTypes.FanOut}", $"routeKey2_{ExchangeTypes.FanOut}");

var rabbitMq2 = new RabbitMqFactory($"exchangeName_{ExchangeTypes.Direct}");
await rabbitMq2.RegisterConsume(ExchangeTypes.Direct, $"queueName2_{ExchangeTypes.Direct}", $"routeKey2_{ExchangeTypes.Direct}");

var rabbitMq3 = new RabbitMqFactory($"exchangeName_{ExchangeTypes.Topic}");
await rabbitMq3.RegisterConsume(ExchangeTypes.Topic, $"queueName2_{ExchangeTypes.Topic}", $"routeKey2_{ExchangeTypes.Topic}.*");

var rabbitMq4 = new RabbitMqFactory($"exchangeName_{ExchangeTypes.Headers}");
await rabbitMq4.RegisterConsume(ExchangeTypes.Headers, $"queueName2_{ExchangeTypes.Headers}", "");

Console.WriteLine("接收訊息");
Console.ReadKey();

Dictionary<string, object?> GetHeaders()
{
    return new Dictionary<string, object?>
    {
        { "x-match", "all" }, // "all"表示所有條件都必須匹配，"any"表示任意一個條件匹配即可
        { "format", "pdf" },
        { "type", "report" }
    };
}