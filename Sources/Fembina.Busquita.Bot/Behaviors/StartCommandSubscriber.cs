using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Localization.Contexts;
using Fembina.Busquita.Storages.Assets;
using Fembina.Busquita.Storages.Caches;
using Talkie.Concurrent;
using Talkie.Controllers.MessageControllers;
using Talkie.Disposables;
using Talkie.Flows;
using Talkie.Handlers;
using Talkie.Pipelines.Handling;
using Talkie.Pipelines.Intercepting;
using Talkie.Signals;
using Talkie.Subscribers;

namespace Fembina.Busquita.Bot.Behaviors;

public sealed class StartCommandSubscriber(IAssetProvider assets) : IBehaviorsSubscriber
{
    private readonly ImageAttachmentCache _aboutImageAttachment = assets.GetAsset("about");

    public void Subscribe(ISignalFlow flow, IRegisterOnlyDisposableScope disposables, CancellationToken cancellationToken)
    {
        flow.Subscribe<MessagePublishedSignal>(signals => signals
            .SkipSelfRelated()
            .OnlyCommands("start")
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
