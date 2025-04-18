using Microsoft.Extensions.Configuration;
using Talkie.Disposables;
using Talkie.Flows;
using Talkie.Subscribers;

namespace Fembina.Busquita.Bot.Integrations;

public sealed class TelegramSubscriber(IConfiguration configuration) : IIntegrationsSubscriber
{
    public async Task SubscribeAsync(ISignalFlow flow, IRegisterOnlyDisposableScope disposables, CancellationToken cancellationToken)
    {
        var token = configuration["Talkie:Telegram:Token"];

        ArgumentNullException.ThrowIfNull(token, "Telegram token is not configured");

        await flow
            .ConnectTelegramAsync(token, cancellationToken)
            .DisposeAsyncWith(disposables);
    }
}
