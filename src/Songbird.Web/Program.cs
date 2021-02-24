using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

[assembly: InternalsVisibleTo("Songbird.Web.Tests")]

namespace Songbird.Web {
    public static class Program {
        public static string AppEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{AppEnvironment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static Int32 Main(string[] args) {
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{RequestId}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    @"D:\home\LogFiles\Application\songbird.txt",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 5_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{RequestId}] {Message:lj}{NewLine}{Exception}",
                    flushToDiskInterval: TimeSpan.FromSeconds(1));

            Log.Logger = loggerConfiguration.CreateLogger();
            try {
                Log.Information("Starting host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            } catch(Exception ex) {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder
                .ConfigureKestrel((_, config) => {
                    // Handle requests up to 50 MB
                    config.Limits.MaxRequestBodySize = 52_428_800;
                })
                .ConfigureAppConfiguration((_, config) => config.AddConfiguration(Configuration))
                    .UseStartup<Startup>();
                });
    }
}
