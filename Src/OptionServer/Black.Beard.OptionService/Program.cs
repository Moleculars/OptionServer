using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bb.OptionService;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bb.OptionService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateWebHostBuilder(args)
                //.UseKestrel(option =>
                //{

                //    option.Listen(IPAddress.Loopback, 5000);
                //    option.Listen(IPAddress.Any, 80);
                //    option.Listen(IPAddress.Loopback, 443, listenOptions =>
                //    {
                //        listenOptions.UseHttps()
                //        listenOptions.UseHttps("", "password");
                //    });

                //})                
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
