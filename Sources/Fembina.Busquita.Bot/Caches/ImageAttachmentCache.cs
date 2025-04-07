using Talkie.Controllers.AttachmentControllers;
using Talkie.Models.Identifiers;
using Talkie.Models.Messages.Attachments;
using Talkie.Models.Messages.Attachments.Factories;
using Talkie.Models.Messages.Attachments.Variants;
using Talkie.Models.Messages.Incoming;

namespace Fembina.Busquita.Bot.Caches;

public sealed class ImageAttachmentCache(string path)
{
    private IMessageAttachmentIdentifier? _identifier;

    public IMessageImageAttachmentFactory Get(IMessageAttachmentController controller)
    {
        ArgumentNullException.ThrowIfNull(controller);

        var identifier = _identifier;

        return identifier is null
            ? controller.ImageAttachment.Build(path)
            : controller.ImageAttachment.Build(identifier);
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
}
