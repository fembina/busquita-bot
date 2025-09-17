using System.Logging.Factories;
using System.Logging.Loggers;
using Falko.Talkie.Controllers.MessageControllers;
using Falko.Talkie.Handlers;
using Falko.Talkie.Models.Messages;
using Falko.Talkie.Models.Messages.Contents;
using Falko.Talkie.Models.Messages.Contents.Styles;
using Falko.Talkie.Models.Messages.Incoming;
using Falko.Talkie.Models.Profiles;
using Falko.Talkie.Signals;

namespace Fembina.Busquita.Bot.Behaviors;

public class HelloCommandHandler : SignalHandler<MessagePublishedSignal>
{
    private static readonly Logger Logger = typeof(HelloCommandHandler).CreateLogger();

    public override async ValueTask HandleAsync(ISignalContext<MessagePublishedSignal> context, CancellationToken cancellationToken)
    {
        var message = context.GetMessage();

        if (message.IsOlderThan(TimeSpan.FromMinutes(3))) return;

        if (message.IsSelfRelated()) return;

        var commandController = context.ToCommandController(message.GetText());

        if (commandController.IsCommand("hello") is false) return;

        var publisherProfile = message.PublisherProfile;
        var publisherName = GetDisplayName(publisherProfile);

        var logIdentifier = Random.Shared.Next();

        Logger.Info("Handling hello command for {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);

        var contentBuilder = new MessageContentBuilder();
        contentBuilder.AddText("Hello, ", BoldTextStyle.FromTextRange);
        contentBuilder.AddText(publisherName);
        var content = contentBuilder.Build();

        var messageController = context.ToMessageController();

        Logger.Info("Publishing hello message to {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
        var outgoingMessage = await messageController.PublishMessageAsync(content, cancellationToken);
        Logger.Info("Published hello message to {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);

        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        Logger.Info("Unpublishing hello message to {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
        var outgoingMessageIdentifier = outgoingMessage.ToGlobalMessageIdentifier();
        await messageController.UnpublishMessageAsync(outgoingMessageIdentifier, cancellationToken);
        Logger.Info("Unpublished hello message to {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
    }

    private string GetDisplayName(IProfile profile)
    {
        const string defaultName = "Unknown";

        var userProfile = profile.AsUserProfile();

        if (userProfile is null) return defaultName;

        var firstName = userProfile.FirstName;

        if (string.IsNullOrWhiteSpace(firstName)) return defaultName;

        return firstName;
    }
}
