using Fembina.Busquita.Bot.Behaviors;
using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Bot.Integrations;
using Fembina.Busquita.Storages.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Talkie.Hosting;

await new HostBuilder()
    .UseConfigurations()
    .UseTalkie(configuration => configuration
        .SetSignalsLogging())
    .ConfigureServices(services => services
        .AddSingleton<IAssetProvider, AssetProvider>()
        .AddBehaviorsSubscriber<StartCommandSubscriber>()
        .AddBehaviorsSubscriber<TrackNameSubscriber>()
        .AddIntegrationsSubscriber<TelegramSubscriber>())
    .ConfigureLogging(logging => logging
        .ClearProviders()
        .AddSerilog(new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/Log.log", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Verbose()
            .CreateLogger(), dispose: true))
    .RunConsoleAsync();
