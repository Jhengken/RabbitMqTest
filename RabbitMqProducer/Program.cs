using Lib;

PrintMsg();
var exchangeType = SelectExchangeType();

// string queueName2 = "DirectQueue2";
var rabbitMq = new RabbitMqFactory($"exchangeName_{exchangeType}");

var routingKey = GetRouteKey(exchangeType);
var bind = GetBind(exchangeType);
var headers = GetHeaders(exchangeType);

// 假設 Consume RoutingKey 設定 queueName_topic.*
// 這裡 RoutingKey 可以設：queueName_topic.AAA、queueName_topic.BBB

await rabbitMq.RegisterProducer(
    exchangeType,
    $"queueName_{exchangeType}",
    IsHeader(exchangeType) ? "" : routingKey,
    bind
    );

// await rabbitMq.DeclareQueueAsync(queueName2, routeKey);

await PublishMsg(rabbitMq, routingKey, headers);

void PrintMsg()
{
    Console.WriteLine("Hello, World!");
    Console.WriteLine("4 Mode");
    Console.WriteLine("enter 1 use fanout");
    Console.WriteLine("enter 2 use direct");
    Console.WriteLine("enter 3 use topic");
    Console.WriteLine("enter 4 use Headers");
    Console.WriteLine("");
}

ExchangeTypes SelectExchangeType()
{
    var readKey = Console.ReadKey().KeyChar;
    return readKey switch
    {
        '1' => ExchangeTypes.FanOut,
        '2' => ExchangeTypes.Direct,
        '3' => ExchangeTypes.Topic,
        '4' => ExchangeTypes.Headers,
        _ => throw new ArgumentException()
    };
}

async Task PublishMsg(RabbitMqFactory rabbitMqFactory, string routeKey, Dictionary<string, object?>? dictionary = null)
{
    Console.WriteLine("""RabbitMQ連接成功,如需離開請按下 "exit" 鍵""");
    do
    {
        var message = Console.ReadLine();
        if (message == "exit")
        {
            return;
        }
        await rabbitMqFactory.PublishAsync(message, routeKey,  dictionary);
    } while (true);
}

Dictionary<string, object?>? GetHeaders(ExchangeTypes exchangeTypes)
{
    return IsHeader(exchangeTypes)
        ? new Dictionary<string, object?>
        {
            { "x-match", "all" }, // "all"表示所有條件都必須匹配，"any"表示任意一個條件匹配即可
            { "format", "pdf" },
            { "type", "report" },
            { "test", "test" },
        }
        : null;
}

Dictionary<string, object?>? GetBind(ExchangeTypes exchangeTypes)
{
    return IsHeader(exchangeTypes)
        ? new Dictionary<string, object?>
        {
            { "x-match", "all" }, // "all"表示所有條件都必須匹配，"any"表示任意一個條件匹配即可
            { "format", "pdf" },
            { "type", "report" },
        }
        : null;
}

bool IsHeader(ExchangeTypes exchangeTypes)
{
    return exchangeTypes == ExchangeTypes.Headers;
}

string GetRouteKey(ExchangeTypes exchangeType2)
{
    return IsTopic(exchangeType2)
        ? $"routeKey_{exchangeType2}.testTopic"
        : $"routeKey_{exchangeType2}";
}

bool IsTopic(ExchangeTypes exchangeType1)
{
    return exchangeType1 == ExchangeTypes.Topic;
}
