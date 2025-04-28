using System.Reflection;
using System.Text;
using Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
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
        services.AddSingleton<IUserService, UserService>();
        
        services.AddControllers();
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddOptions();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
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
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseCors();
        
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        
    }
}
