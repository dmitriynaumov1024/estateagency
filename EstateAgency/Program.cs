using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Client;

namespace EstateAgency
{
    public static class Db
    {
        public static IIgniteClient Client;
        public static void Connect()
        {
            var cfg = new IgniteClientConfiguration { 
                Endpoints = new[] {"127.0.0.1:10800"}
            };
            Client = Ignition.StartClient (cfg);
        }
        public static void Disconnect()
        {
            Client.Dispose();
            Client = null;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Db.Connect();
            IHost host = CreateHostBuilder(args).Build();
            host.Run();
            Console.WriteLine("Shutdown");
            System.Threading.Thread.Sleep(1000);
            Db.Disconnect();
            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
