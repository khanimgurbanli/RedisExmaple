
using StackExchange.Redis;

//create redis connection 
ConnectionMultiplexer connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost:1453");

ISubscriber subscriber = connectionMultiplexer.GetSubscriber();

while (true)
{
    Console.Write("Message: ");
    string message = Console.ReadLine();
    await subscriber.PublishAsync("mychannel", message);
}