using StackExchange.Redis;

namespace Redis.Sentinels.Services
{
    public class RedisSentinelService
    {
        //Download: Microsoft.Extensions.Caching.Distributed.IDistributedCache  package

        public static ConfigurationOptions ConfigurationOptions => new()
        {
            EndPoints =
           {
                {"localhost", 6383 },
                {"localhost", 6384 },
                {"localhost", 6385 }
           },
            CommandMap = CommandMap.Sentinel,
            AbortOnConnectFail = false
        };

        public static ConfigurationOptions masterOptions => new()
        {
            AbortOnConnectFail = false
        };

        public static async Task<IDatabase> RedisMasterDb()
        {
            System.Net.EndPoint masterEnpoint = null;

            ConnectionMultiplexer connectionMultiplex = await ConnectionMultiplexer.SentinelConnectAsync(ConfigurationOptions);

            foreach (System.Net.EndPoint endPoint in connectionMultiplex.GetEndPoints())
            {
                IServer server = connectionMultiplex.GetServer(endPoint);

                if (!server.IsConnected)
                    continue;

                masterEnpoint = await server.SentinelGetMasterAddressByNameAsync("mymaster");//sentinel name which in sentinel.conf  sentinel server name
                break;
            }

            var localMasterIP = masterEnpoint.ToString() switch
            {
                "192.168.96.2:6379" => "localhost:6379",
                "192.168.96.3:6379" => "localhost:6380",
                "192.168.96.4:6379" => "localhost:6381",
                "192.168.96.5:6379" => "localhost:6382",
                "192.168.96.6:6379" => "localhost:6383",
                "192.168.96.7:6379" => "localhost:6384",
                "192.168.96.8:6379" => "localhost:6385",
            };

            ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);

            IDatabase database = masterConnection.GetDatabase();

            return database;
        }
    }
}
