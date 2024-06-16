using BLL.Contracts;
using BLL.Services;
using GateController.Forms;
using Infrastructure.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GateController.Infrastructure.Extensions;
public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureServices(
        this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IClient, Client>();
            services.AddTransient<IParkingSessionService, ParkingSessionService>();
            services.AddTransient<ISerialClient, SerialClient>();
        });
    }

    public static IHostBuilder ConfigureForms(
        this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddTransient<Main>();
            services.AddTransient<Login>();
        });
    }

    public static IHostBuilder SetupConfiguration(
        this IHostBuilder hostBuilder)
    {
        var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
           .AddJsonFile("appsettings.json")
           .Build();

        var serverSettings = configuration
            .GetSection("ServerSettings")
            .Get<ServerSettings>();

        var plateRecognitionConfig = configuration
            .GetSection("PlateRecognitionConfig")
            .Get<PlateRecognitionConfig>();

        return hostBuilder.ConfigureServices((context, services) =>
        {
            services.AddSingleton(serverSettings!);
            services.AddSingleton(plateRecognitionConfig!);
        });
    }
}
