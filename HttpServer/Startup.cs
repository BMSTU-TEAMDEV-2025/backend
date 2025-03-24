using System.Reflection;
using Core.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Mongo.Extensions;
using Services;

namespace Backend;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
        
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        
        services.AddMongoService(Configuration, Assembly.Load("Core"));

        services.AddSingleton<IHelloService, HelloService>();
        
        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddOptions();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseRouting();
        
        app.UseCors();
        
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}
