using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

namespace ProjectTemplate.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = CreateSerilogLogger(BuildConfiguration());

            try
            {
                Log.Information("Starting Host");
                CreateHostBuilder(args).Build().Run();
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services => 
                {
                    services.AddPostgresHealthCheck(BuildConfiguration());
                    services.AddMartenDB(BuildConfiguration());
                }).UseSerilog(logger: Log.Logger);

        private static IConfiguration BuildConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            return logger;
        }
    }
}
