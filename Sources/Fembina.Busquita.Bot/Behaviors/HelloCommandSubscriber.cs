using Falko.Talkie.Disposables;
using Falko.Talkie.Flows;
using Falko.Talkie.Pipelines.Handling;
using Falko.Talkie.Pipelines.Intercepting;
using Falko.Talkie.Subscribers;

namespace Fembina.Busquita.Bot.Behaviors;

public class HelloCommandSubscriber : IBehaviorsSubscriber
{
    public void Subscribe(ISignalFlow flow, IRegisterOnlyDisposableScope disposables, CancellationToken cancellationToken)
    {
        var handler = new HelloCommandHandler();

        var handlingPipeline = new SingleSignalHandlingPipeline(handler);

        flow.Subscribe(handlingPipeline).UnsubscribeWith(disposables);
    }
}
