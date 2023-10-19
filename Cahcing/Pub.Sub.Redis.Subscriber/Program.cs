
using StackExchange.Redis;


/*d
 Dowbload: StackExchange.Redis package 

 ----------> Docker command <------------------
docker ps- active conatiners list
docker network ls - networks list
docker network rm networkname - remove connection
docker inspect --format='{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}'<<container_name>> - learn conainer extermal ip
docker network create redis-network (name-> redis-network ) - create docker network
create master redis server --> docker run -d --name redis-master -p 6379:6379 --network redis-network redis redis-server (for remove-> docker rm redis-master for stop-> docker stop redis-master)
create slave redis server  --> docker run -d --name redis-slave1 -p 6380:6379 --network redis-network redis redis-server --slaveof redis-master 6379

*/

/*
----------> configuration Sentinal Severver <-------------
IP-ni oyren docker inspect --format='{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' containername

    sentinel monitor <name of redis cache>  <server IP> <port> 2
    sentinel down-after-milliseconds <name of redis cache> 4000
    sentinel failover-timeout <name of redis cache> 180000
    sentinel parallel-syncs <name of redis cache> 1

create sentinel 
docker run -d --name redis-sentinel-1 -p 6383:26379 --network redis-network -v D:\ChachingOperations\redis-sentinel\sentinel.conf:/usr/local/etc/redis/sentinel.conf redis redis-sentinel /usr/local/etc/redis/sentinel.conf
 */


ConnectionMultiplexer connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync("localhost:1453");

ISubscriber subscriber = connectionMultiplexer.GetSubscriber();
//this is a channel --> subscribe psubscribe mychannel.*
await subscriber.SubscribeAsync("mychannel", (channel, message) =>
{
    Console.WriteLine(message);
});


//this is a pattern --> publish mychannel.* hello
await subscriber.SubscribeAsync("mychannel.*", (channel, message) =>// or channelname.x pattern
{
    Console.WriteLine(message);
});

Console.Read();
