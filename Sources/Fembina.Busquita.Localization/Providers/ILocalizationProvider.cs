using Fembina.Busquita.Localization.Variants;
using Falko.Talkie.Localizations;

namespace Fembina.Busquita.Localization.Providers;

public interface ILocalizationProvider
{
    ILocalization GetOrDefault(Language language);
}
