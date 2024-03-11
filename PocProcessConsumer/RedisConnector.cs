using StackExchange.Redis;

namespace PocProcessConsumer
{
    public static class RedisConnectorHelper
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

        static RedisConnectorHelper()
        {
            var configurationOptions = new ConfigurationOptions
            {
                EndPoints = { "redis:6379" },
                User = "redis"
            };
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
        }

        public static ConnectionMultiplexer Connection => LazyConnection.Value;
    }
}