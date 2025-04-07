using Fembina.Busquita.Bot.Caches;
using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Localization.Contexts;
using Fembina.Busquita.Storages.Assets;
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
    private readonly ImageAttachmentCache _aboutImageCache = new(assets.GetAsset("about"));

    public void Subscribe(ISignalFlow flow, IRegisterOnlyDisposableScope disposables, CancellationToken cancellationToken)
    {
        flow.Subscribe<MessagePublishedSignal>(signals => signals
            .SkipSelfRelated()
            .OnlyCommand("start")
            .HandleAsync((context, cancellation) => context
                .ToMessageController()
                .PublishMessageAsync(message => message
                    .SetContent(context.Localize(localization => localization.About))
                    .AddAttachment(_aboutImageCache.GetOrCreate(context)), cancellation)
                .HandleOnSuccess(_aboutImageCache.TrySet)
                .AsValueTask()))
            .UnsubscribeWith(disposables);
    }
}
