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
        // GET api/values/key1
        [HttpGet("{key}")]
        public string Get(string key)
        {
            var clientConfig = new ClientConfig();
            clientConfig
                .GetNetworkConfig()
                .AddAddress("0.0.0.0:5705", "0.0.0.0:5706");
            //.AddAddress("0.0.0.0:5705");
            clientConfig.SetGroupConfig(new GroupConfig("hc-farm","s3crEt"));

            //client with custom configuration
            IHazelcastInstance client = HazelcastClient.NewHazelcastClient(clientConfig);
            var map = client.GetMap<string,string>("mymap");
            var result = map.Get(key);
            return result;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] RequestData data)
        {
            var clientConfig = new ClientConfig();
            clientConfig
                .GetNetworkConfig()
                .AddAddress("0.0.0.0:5705","0.0.0.0:5706");
            //.AddAddress("0.0.0.0:5705");
            clientConfig.SetGroupConfig(new GroupConfig("hc-farm","s3crEt"));
            
            //client with custom configuration
            IHazelcastInstance client = HazelcastClient.NewHazelcastClient(clientConfig);
            var map = client.GetMap<string,string>("mymap");
            map.Put(data.Key, data.Value);
            return "ok";
        }
    }
    
    public class RequestData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}