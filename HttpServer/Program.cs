using NLog.Web;

namespace Backend;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder(args);
        var app = builder.Build();
        app.Run();
    }
    
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
            builder.UseKestrel();
            builder.UseNLog();
        });
    }
}
