using Falko.Talkie.Controllers.AttachmentControllers;
using Falko.Talkie.Handlers;
using Falko.Talkie.Models.Identifiers;
using Falko.Talkie.Models.Messages.Attachments;
using Falko.Talkie.Models.Messages.Attachments.Factories;
using Falko.Talkie.Models.Messages.Attachments.Variants;
using Falko.Talkie.Models.Messages.Incoming;
using Falko.Talkie.Signals;

namespace Fembina.Busquita.Storages.Caches;

public sealed class ImageAttachmentCache(string path)
{
    private IMessageAttachmentIdentifier? _identifier;

    public IMessageImageAttachmentFactory GetOrCreate(IMessageAttachmentController controller)
    {
        ArgumentNullException.ThrowIfNull(controller);

        var identifier = _identifier;


        return identifier is null
            ? controller.ImageAttachment.Build(path)
            : controller.ImageAttachment.Build(identifier);
    }

    public IMessageAttachmentFactory GetOrCreate(ISignalContext<MessagePublishedSignal> context)
    {
        return GetOrCreate(context.GetAttachmentController());
    }

    public IMessageAttachmentFactory GetOrCreate(ISignalContext<MessageExchangedSignal> context)
    {
        return GetOrCreate(context.GetAttachmentController());
    }

    public void TrySet(IIncomingMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (_identifier is not null) return;

        _identifier = message
            .Attachments
            .GetImages()
            .First()
            .Variants
            .GetImages()
            .OrderByHighest()
            .First()
            .Identifier;
    }

    public static implicit operator ImageAttachmentCache(string path) => new(path);
}
