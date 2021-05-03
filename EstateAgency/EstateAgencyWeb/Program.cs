using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EstateAgency.Database;

namespace EstateAgencyWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DbClient.Connect();
            DbClient.SetClusterActive(true);
            DbClient.GetDatabase();

            CreateHostBuilder(args).Build().Run();
            DbClient.Disconnect();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
