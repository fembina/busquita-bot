using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Fembina.Busquita.Bot.Extensions;

public static class ConfigurationExtensions
{
    public static IHostBuilder UseConfigurations(this IHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddJsonFile("config.json");
#if DEBUG
            config.AddJsonFile("config.dev.json", optional: true);
#endif
        });
    }
}
