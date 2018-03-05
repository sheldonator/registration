using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace registration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            ConfigureWebHostBuilder(args)
                .Build();

        public static IWebHostBuilder ConfigureWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseSerilog(ConfigureLogging)
                .UseStartup<Startup>();
        }

        private static void ConfigureLogging(WebHostBuilderContext ctx, LoggerConfiguration config)
        {
            config.WriteTo.Async(a =>
            {
                a.File("Registration-[TODAY]".Replace("[TODAY]", DateTime.Now.Date.ToString("yyyy-MM-dd")),
                        outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] CorrelationId: {CorrelationId} Message: {Message}{NewLine}{Exception}")
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext();
            });
        }
    }
}
