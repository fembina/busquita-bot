using Fembina.Busquita.Localization.Contexts;
using Falko.Talkie.Models.Messages.Contents;

namespace Fembina.Busquita.Localization.Factories;

public sealed class LocalizationContent : ILocalizationContent
{
    private readonly Func<LocalizationContext, MessageContent> _factory;

    private LocalizationContent(Func<LocalizationContext, MessageContent> factory) => _factory = factory;

    public MessageContent Localize(LocalizationContext context) => _factory(context);

    public static ILocalizationContent Factory(Func<LocalizationContext, MessageContent> factory)
    {
        return new LocalizationContent(factory);
    }

    public static ILocalizationContent Factory(Func<LocalizationContext, IMessageContentBuilder, IMessageContentBuilder> factory)
    {
        return new LocalizationContent(context => factory(context, new MessageContentBuilder()).Build());
    }

    public static ILocalizationContent Instance(MessageContent content)
    {
        return new LocalizationContent(_ => content);
    }
}
