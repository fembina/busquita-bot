using Fembina.Busquita.Bot.Extensions;
using Fembina.Busquita.Bot.Utils;
using Talkie.Controllers.MessageControllers;
using Talkie.Disposables;
using Talkie.Flows;
using Talkie.Handlers;
using Talkie.Pipelines.Handling;
using Talkie.Pipelines.Intercepting;
using Talkie.Signals;
using Talkie.Subscribers;

namespace Fembina.Busquita.Bot.Behaviors;

public sealed class TrackNameSubscriber : IBehaviorsSubscriber
{
    public void Subscribe(ISignalFlow flow, IRegisterOnlyDisposableScope disposables, CancellationToken cancellationToken)
    {
        flow.Subscribe<MessagePublishedSignal>(signals => signals
            .SkipSelfRelated()
            .SkipCommands()
            .HandleAsync(HandleTackNameAsync))
            .UnsubscribeWith(disposables);
    }

    private static async ValueTask HandleTackNameAsync(ISignalContext<MessagePublishedSignal> context, CancellationToken cancellationToken)
    {
        var content = context.GetMessage().Content;

        if (content.IsEmpty) return;

        var trackName = TrackNameFormatter.FormatTextToTrackName(content.Text);

        await context
            .ToMessageController()
            .PublishMessageAsync(trackName.Length >= TrackNameFormatter.TrackMinLength
                ? $"Track name is '{trackName}'"
                : "Track name too short", cancellationToken);
    }
}
