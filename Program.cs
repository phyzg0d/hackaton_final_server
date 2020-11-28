using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServerAspNetCoreLinux.Lib.fastJSON;
using ServerAspNetCoreLinux.ServerCore;
using ServerAspNetCoreLinux.ServerCore.ServerLogger;

namespace ServerAspNetCoreLinux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configServer = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            try
            {
                ServerLoggerModel.Log(TypeLog.Info, "server is starting");
                CreateHostBuilder(configServer).Build().Run();
            }
            catch (Exception e)
            {
                ServerLoggerModel.Log(TypeLog.Fatal, $"launch server error: {e}");
            }
        }

        public static IWebHostBuilder CreateHostBuilder(IConfigurationRoot args) => new WebHostBuilder()
            .UseConfiguration(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel()
            .UseIISIntegration()
            .SuppressStatusMessages(true)
            .ConfigureLogging((context, logging) => { logging.ClearProviders(); })
            .UseStartup<Startup>();
    }
}