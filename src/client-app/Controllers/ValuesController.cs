using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hazelcast.Client;
using Hazelcast.Config;
using Hazelcast.Core;
using Microsoft.AspNetCore.Mvc;

namespace ClientApplication.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ValuesController()
        {
            Console.WriteLine("ctor");
        }
        // GET api/values/key1
        [HttpGet("{key}")]
        public string Get(string key)
        {
            var clientConfig = new ClientConfig();
            clientConfig
                .GetNetworkConfig()
                .AddAddress("0.0.0.0:5705"
                    , "0.0.0.0:5706");
            //.AddAddress("0.0.0.0:5705");
            clientConfig.SetGroupConfig(new GroupConfig("hc-farm", "s3crEt"));

            //client with custom configuration
            Console.WriteLine(DateTime.Now.ToString() + ": GET > Connecting Hazelcast");
            IHazelcastInstance client = HazelcastClient.NewHazelcastClient(clientConfig);
            Console.WriteLine(DateTime.Now.ToString() + ": GET > Connected " + client.GetName());

            var map = client.GetMap<string, string>("mymap");

            Console.WriteLine(DateTime.Now.ToString() + ": GET > Retrieving value");
            var result = map.Get(key);
            Console.WriteLine(DateTime.Now.ToString() + ": GET > Retrieved value");

            return result;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] RequestData data)
        {
            var clientConfig = new ClientConfig();
            clientConfig
                .GetNetworkConfig()
                .AddAddress("0.0.0.0:5705"
                , "0.0.0.0:5706");
            //.AddAddress("0.0.0.0:5705");
            clientConfig.SetGroupConfig(new GroupConfig("hc-farm", "s3crEt"));

            //client with custom configuration
            Console.WriteLine(DateTime.Now.ToString() + ": POST > Connecting Hazelcast");
            IHazelcastInstance client = HazelcastClient.NewHazelcastClient(clientConfig);
            Console.WriteLine(DateTime.Now.ToString() + ": POST > Connected " + client.GetName());

            var map = client.GetMap<string, string>("mymap");

            Console.WriteLine(DateTime.Now.ToString() + ": POST > Starting to set value");
            map.Put(data.Key, data.Value);
            Console.WriteLine(DateTime.Now.ToString() + ": POST > Set value");

            return "ok";
        }
    }

    public class RequestData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}