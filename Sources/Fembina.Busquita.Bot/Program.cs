using System.Logging.Builders;
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
using Microsoft.Extensions.Logging;
using Falko.Talkie.Hosting;

await new HostBuilder()
    .UseConfigurations()
    .UseTalkie(configuration => configuration
        .SetSignalsLogging())
    .ConfigureServices(services => services
        .AddSingleton<IAssetProvider, AssetProvider>()
        .AddBehaviorsSubscriber<StartCommandSubscriber>()
        .AddIntegrationsSubscriber<TelegramSubscriber>())
    .ConfigureLogging(ConfigureLogging)
    .RunConsoleAsync();

return;

static void ConfigureLogging(ILoggingBuilder logging)
{
    var loggerRuntime = LoggerRuntime.Global;

    var logLevel = LogLevels.TraceAndAbove;

    loggerRuntime.Initialize(new LoggerContextBuilder().SetLevel(logLevel)
        .SetLevel(logLevel)
        .AddTarget(SimpleLogContextRenderer.Instance,
            LoggerConsoleTarget.Instance)
        .AddTarget(SimpleLogContextRenderer.Instance,
            new LoggerFileTarget("busquita", "./Logs")));

    logging.ClearProviders()
        .SetMinimumLevel(logLevel.ToMinimumLogLevel())
        .AddProvider(loggerRuntime.ToMicrosoftLoggerProvider(true));
}
