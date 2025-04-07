using Fembina.Busquita.Localization.Variants;
using Talkie.Localizations;

namespace Fembina.Busquita.Localization.Providers;

public sealed class LocalizationProvider : ILocalizationProvider
{
    public static readonly ILocalizationProvider Instance = new LocalizationProvider();

    private LocalizationProvider() { }

    public ILocalization GetOrDefault(Language language) => language switch
    {
        Language.Russian => Get<RussianLocalization>(),
        Language.Ukrainian => Get<UkrainianLocalization>(),
        Language.English => Get<EnglishLocalization>(),
        Language.Spanish => Get<SpanishLocalization>(),
        _ => Get<EnglishLocalization>()
    };

    private static T Get<T>() where T : class, ILocalization, new() => LocalizationCache<T>.Instance;
}

file static class LocalizationCache<T> where T : class, ILocalization, new()
{
    public static readonly T Instance = new();
}
