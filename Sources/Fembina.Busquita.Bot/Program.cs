using System.Logging.Builders;
using System.Logging.Loggers;
using System.Logging.Logs;
using System.Logging.Providers;
using System.Logging.Renderers;
using System.Logging.Runtimes;
using System.Logging.Targets;
using Fembina.Busquita.Bot.Behaviors;
using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Bot.Integrations;
using Fembina.Busquita.Storages.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Falko.Talkie.Hosting;
using LoggerFactory = System.Logging.Factories.LoggerFactory;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

using var loggerRuntime = LoggerRuntime.Global;

loggerRuntime.Initialize(new LoggerContextBuilder()
    .SetLevel(LogLevels.TraceAndAbove)
    .AddTarget(SimpleLogContextRenderer.Instance, LoggerConsoleTarget.Instance)
    .AddTarget(SimpleLogContextRenderer.Instance, new LoggerFileTarget("busquita", "./Logs")));

await new HostBuilder()
    .UseConfigurations()
    .UseTalkie(configuration => configuration
        .SetSignalsLogging())
    .ConfigureServices(services => services
        .AddSingleton<IAssetProvider, AssetProvider>()
        .AddBehaviorsSubscriber<StartCommandSubscriber>()
        .AddIntegrationsSubscriber<TelegramSubscriber>())
    .ConfigureLogging(logging => logging
        .ClearProviders()
        .SetMinimumLevel(LogLevel.Trace)
        .AddProvider(new MicrosoftLoggerProvider(loggerRuntime, false)))
    .RunConsoleAsync();
