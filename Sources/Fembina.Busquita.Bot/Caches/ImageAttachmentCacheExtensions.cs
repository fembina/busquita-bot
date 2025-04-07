using Talkie.Handlers;
using Talkie.Models.Messages.Attachments.Factories;
using Talkie.Signals;

namespace Fembina.Busquita.Bot.Caches;

public static class ImageAttachmentCacheExtensions
{
    public static IMessageAttachmentFactory Get(this ImageAttachmentCache cache, ISignalContext<MessagePublishedSignal> context)
    {
        return cache.Get(context.GetAttachmentController());
    }
}
