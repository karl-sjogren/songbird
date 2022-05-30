using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

[assembly: InternalsVisibleTo("Songbird.Web.Tests")]

namespace Songbird.Web;

public static class Program {
    public static string AppEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{AppEnvironment}.json", optional: true, reloadOnChange: true)
        .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

    public static string GetAssemblyVersion() {
        return typeof(Program).Assembly.GetName().Version.ToString(3);
    }

    public static Int32 Main(string[] args) {
        // Art by Joan Stark, https://www.asciiart.eu/animals/birds-land
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("                 _  _");
        Console.WriteLine(@"                ( \/ )");
        Console.WriteLine(@"         .---.   \  /   .-""-. ");
        Console.WriteLine(@"        /   6_6   \/   / 4 4 \");
        Console.WriteLine(@"        \_  (__\       \_ v _/                 Songbird " + GetAssemblyVersion());
        Console.WriteLine(@"        //   \\        //   \\");
        Console.WriteLine("       ((     ))      ((     ))");
        Console.WriteLine(@"jgs=====""""===""""========""""===""""=======");
        Console.WriteLine("          |||            |||");
        Console.WriteLine("           |              |");

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .Enrich.FromLogContext()
            .Filter.ByExcluding(ExcludeEFCoreBasedOnQuery)
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

    private static bool ExcludeEFCoreBasedOnQuery(LogEvent logEvent) {
        if(!logEvent.Properties.ContainsKey("commandText"))
            return false;

        var commandText = logEvent.Properties["commandText"];
        if(commandText != null) {
            if(commandText.ToString().Contains("[DISABLE_EF_LOGGING]"))
                return true;
        }

        return false;
    }
}
