using System.Logging.Logs;
using System.Logging.Renderers;
using System.Logging.Runtimes;
using System.Logging.Targets;
using System.Logging.Builders;
using Falko.Talkie.Hosting;
using Fembina.Busquita.Bot.Behaviors;
using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Bot.Integrations;
using Fembina.Busquita.Storages.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        .SetShutdownOnUnobservedExceptions())
    .ConfigureServices(services => services
        .AddSingleton<IAssetProvider, AssetProvider>()
        .AddBehaviorsSubscriber<HelloCommandSubscriber>()
        .AddBehaviorsSubscriber<StartCommandSubscriber>()
        .AddIntegrationsSubscriber<TelegramSubscriber>())
    .RunConsoleAsync();
