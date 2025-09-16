using System.Logging.Factories;
using System.Logging.Runtimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fembina.Busquita.Bot.Extensions;

public static class HostExtensions
{
    public static IHostBuilder UseZeroLogger(this IHostBuilder builder, LoggerRuntime loggerRuntime)
    {
        builder.ConfigureServices(services =>
        {
            var loggerFactory = new MicrosoftLoggerFactory(loggerRuntime, disposeLoggerRuntime: false);

            services.RemoveAll<ILoggerFactory>();
            services.AddSingleton<ILoggerFactory>(loggerFactory);

            services.RemoveAll(typeof(ILogger<>));
            services.AddSingleton(typeof(ILogger<>), typeof(MicrosoftFactoryLogger<>));

            services.TryAddSingleton(loggerRuntime);
        });

        return builder;
    }
}
