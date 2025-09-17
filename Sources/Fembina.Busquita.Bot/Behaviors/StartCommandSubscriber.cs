using System.Logging.Factories;
using System.Logging.Loggers;
using Fembina.Busquita.Localization.Contexts;
using Fembina.Busquita.Storages.Assets;
using Fembina.Busquita.Storages.Caches;
using Falko.Talkie.Concurrent;
using Falko.Talkie.Controllers.MessageControllers;
using Falko.Talkie.Disposables;
using Falko.Talkie.Flows;
using Falko.Talkie.Handlers;
using Falko.Talkie.Pipelines.Handling;
using Falko.Talkie.Pipelines.Intercepting;
using Falko.Talkie.Signals;
using Falko.Talkie.Subscribers;

namespace Fembina.Busquita.Bot.Behaviors;

public sealed class StartCommandSubscriber(IAssetProvider assets) : IBehaviorsSubscriber
{
    private static readonly Logger Logger = typeof(StartCommandSubscriber).CreateLogger();

    private readonly ImageAttachmentCache _aboutImageAttachment = assets.GetAsset("about");

    public void Subscribe(ISignalFlow flow, IRegisterOnlyDisposableScope disposables, CancellationToken cancellationToken)
    {
        flow.Subscribe<MessagePublishedSignal>(signals => signals
            .SkipSelfRelated()
            .OnlyCommands("start")
            .Handle(context => Logger
                .Debug("Handling start command for environment {Environment}",
                    context.GetMessage().EnvironmentProfile))
            .HandleAsync((context, cancellation) => context
                .ToMessageController()
                .PublishMessageAsync(message => message
                    .SetContent(context.Localize(localization => localization.About))
                    .AddAttachment(_aboutImageAttachment.GetOrCreate(context)), cancellation)
                .HandleOnSuccess(_aboutImageAttachment.TrySet)
                .AsValueTask()))
            .UnsubscribeWith(disposables);
    }
}
