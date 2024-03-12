using Microsoft.AspNetCore;

namespace GraphCalendar.Host;

public class Program
{
    public static async Task Main(string[] args)
    {
        var webHost = BuildWebHost(args);
        await InitWebServices(webHost);
            
    }
        
    private static async Task<int> InitWebServices(IWebHost webHost)
    {
        await Task.WhenAll(
            webHost.RunAsync()
        );
        await Task.WhenAll(
            webHost.StopAsync()
        );
        Environment.Exit(0);
        return 0;
    }
        
    public static IWebHost BuildWebHost(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
    }
}
