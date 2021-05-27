using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Entities;

namespace WebClient
{
    public class Program
    {
        public static DbClient client;

        public static void Main(string[] args)
        {
            client = new DbClient("URI=file:database.db");
            client.Connect();
            client.Execute("pragma foreign_keys=ON;");
            //client.Execute(File.ReadAllText("DbCreate.txt"));
            //DbTest.AddLocations();
            //DbTest.AddPersons();
            CreateHostBuilder(args).Build().Run();
            client.Disconnect();
            Console.WriteLine("Disconnected from db.");
            Thread.Sleep(1000);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
