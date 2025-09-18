using System.Logging.Factories;
using System.Logging.Loggers;
using Falko.Talkie.Controllers.AttachmentControllers;
using Falko.Talkie.Handlers;
using Falko.Talkie.Models.Messages;
using Falko.Talkie.Models.Messages.Contents;
using Falko.Talkie.Models.Messages.Contents.Styles;
using Falko.Talkie.Models.Messages.Features;
using Falko.Talkie.Models.Messages.Incoming;
using Falko.Talkie.Models.Messages.Outgoing;
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

        var attachmentController = context.GetAttachmentController();
        var imageController = attachmentController.ImageAttachment;
        var image = imageController.Build("https://placecats.com/300/200");

        var messageIdentifier = message.ToGlobalMessageIdentifier();

        var messageBuilder = new OutgoingMessageBuilder();
        messageBuilder.SetReply(messageIdentifier);
        messageBuilder.AddContent(content);
        messageBuilder.AddFeature(SilenceMessageFeature.Instance);
        messageBuilder.AddAttachment(image);
        var publishingMessage = messageBuilder.Build();

        var messageController = context.ToMessageController();

        Logger.Info("Publishing hello message to {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
        var outgoingMessage = await messageController.PublishMessageAsync(publishingMessage, cancellationToken);
        Logger.Info("Published hello message to {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);

        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        Logger.Info("Unpublishing hello message for {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
        var outgoingMessageIdentifier = outgoingMessage.ToGlobalMessageIdentifier();
        await messageController.UnpublishMessageAsync(outgoingMessageIdentifier, cancellationToken);
        Logger.Info("Unpublished hello message for {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);

        Logger.Info("Unpublishing hello command message from {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
        await messageController.UnpublishMessageAsync(messageIdentifier, cancellationToken);
        Logger.Info("Unpublished hello command message from {PublisherDisplayName}:{LogIdentifier}", publisherName, logIdentifier);
    }

    private string GetDisplayName(IProfile profile)
    {
        const string defaultName = "Anonymous";

        var userProfile = profile.AsUserProfile();

        if (userProfile is null) return defaultName;

        var firstName = userProfile.FirstName;

        if (string.IsNullOrWhiteSpace(firstName)) return defaultName;

        return firstName;
    }
}
