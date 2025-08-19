using Fembina.Busquita.Localization.Factories;
using Fembina.Busquita.Localization.Providers;
using Fembina.Busquita.Localization.Variants;
using Falko.Talkie.Handlers;
using Falko.Talkie.Signals;

namespace Fembina.Busquita.Localization.Contexts;

public static class SignalContextExtensions
{
    public static LocalizedMessageContentBuilder Localize(this ISignalContext<MessagePublishedSignal> context, Func<ILocalization, ILocalizationContent> selector)
    {
        var language = context
            .GetMessage()
            .PublisherProfile
            .Language;

        var localization = LocalizationProvider
            .Instance
            .GetOrDefault(language);

        return new LocalizedMessageContentBuilder(selector(localization));
    }
}
