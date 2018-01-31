using System;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;

namespace ClientApplication
{
    public class CacheClient
    {
        private static readonly Lazy<IHazelcastInstance> Lazy =
            new Lazy<IHazelcastInstance>(() =>
            {
                Console.WriteLine("Created client");
                
                var clientConfig = new ClientConfig();
                clientConfig
                    .GetNetworkConfig()
                    .AddAddress("0.0.0.0:5705"
                        , "0.0.0.0:5706");
                //.AddAddress("0.0.0.0:5705");
                clientConfig.SetGroupConfig(new GroupConfig("hc-farm", "s3crEt"));
                return HazelcastClient.NewHazelcastClient(clientConfig);
            });
        
        public static IHazelcastInstance Instance => Lazy.Value;
    }
}