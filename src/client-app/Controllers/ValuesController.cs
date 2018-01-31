using System;
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
            IHazelcastInstance client = CacheClient.Instance;
            Console.WriteLine(DateTime.Now + ": GET > Connected " + client.GetName());

            var map = client.GetMap<string, string>("mymap");

            Console.WriteLine(DateTime.Now + ": GET > Retrieving value");
            var result = map.Get(key);
            Console.WriteLine(DateTime.Now + ": GET > Retrieved value");

            return result;
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] RequestData data)
        {
            //client with custom configuration
            IHazelcastInstance client = CacheClient.Instance;
            Console.WriteLine(DateTime.Now + ": POST > Connected " + client.GetName());

            var map = client.GetMap<string, string>("mymap");

            Console.WriteLine(DateTime.Now + ": POST > Starting to set value");
            map.Put(data.Key, data.Value);
            Console.WriteLine(DateTime.Now + ": POST > Set value");

            return "ok";
        }
    }

    public class RequestData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}