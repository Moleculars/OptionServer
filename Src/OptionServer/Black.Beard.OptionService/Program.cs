using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Bb.OptionService
{
    public class Program
    {
        public static void Main(string[] args)
        {

            InitializeLog();
            try
            {
                Log.Information("Starting web host");

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
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }


        }

        private static void InitializeLog()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            var listeners = System.Diagnostics.Trace.Listeners;
            listeners.Clear();
            var listener = new SerilogTraceListener.SerilogTraceListener()
            {

            };
            listeners.Add(listener);


            System.Diagnostics.Trace.WriteLine("Log");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
.UseSerilog()
.UseStartup<Startup>();
        }
    }
}
