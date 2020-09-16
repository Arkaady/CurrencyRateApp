using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurrencyRateApp.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CurrencyRateApp
{
    public static class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();


        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .Enrich.FromLogContext()
                   .WriteTo.Debug()
                   .CreateLogger();

                Log.Information("Starting host...");

                var host = CreateHostBuilder(args).Build();
                SeedDatabase(host);
                host.Run();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SeedDatabase(IHost host)
        {
            using var oScope = host.Services.CreateScope();
            var oServices = oScope.ServiceProvider;
            try
            {
                var context = oServices.GetRequiredService<EFContext>();
                var seedManager = new SeedManager(context);
                seedManager.SeedDatabaseAsync().GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "An error occurred while seeding the database.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) 
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseSerilog();
                });
    }
}
