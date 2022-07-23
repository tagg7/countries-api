using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Countries.Web.Tests.Setup;

public class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override IHostBuilder? CreateHostBuilder()
    {
        var settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.json");
        
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((_, cfg) =>
            {
                cfg.AddJsonFile(settingsPath);
                cfg.AddEnvironmentVariables();
                cfg.Build();
            })
            .ConfigureWebHostDefaults(x =>
            {
                x
                    .UseEnvironment("Test")
                    .UseStartup<Startup>()
                    .UseTestServer();
            });

        return builder;
    }
    
    public TService GetService<TService>()
    {
        var service = Services.GetServices<TService>().FirstOrDefault()
                      ?? throw new Exception($"No registration exists for type {typeof(TService)}");
        return service;
    }
}