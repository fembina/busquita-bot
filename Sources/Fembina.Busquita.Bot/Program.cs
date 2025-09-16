using System.Logging.Logs;
using System.Logging.Renderers;
using System.Logging.Runtimes;
using System.Logging.Targets;
using Fembina.Busquita.Bot.Behaviors;
using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Bot.Integrations;
using Fembina.Busquita.Storages.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Falko.Talkie.Hosting;

using var loggerRuntime = LoggerRuntime.Global;

loggerRuntime.Initialize(builder => builder
    .SetLevel(LogLevels.TraceAndAbove)
    .AddTarget(SimpleLogContextRenderer.Instance,
        new LoggerFileTarget("busquita", "./Logs")
            .AsConcurrent())
    .AddTarget(SimpleLogContextRenderer.Instance,
        LoggerConsoleTarget.Instance
            .AsConcurrent()));

await new HostBuilder()
    .UseConfigurations()
    .UseZeroLogger(loggerRuntime)
    .UseTalkie(configuration => configuration
        .SetShutdownOnUnobservedExceptions()
        .SetSignalsLogging())
    .ConfigureServices(services => services
        .AddSingleton<IAssetProvider, AssetProvider>()
        .AddBehaviorsSubscriber<StartCommandSubscriber>()
        .AddIntegrationsSubscriber<TelegramSubscriber>())
    .RunConsoleAsync();
