using Fembina.Busquita.Localization.Contexts;
using Falko.Talkie.Models.Messages.Contents;

namespace Fembina.Busquita.Localization.Factories;

public interface ILocalizationContent
{
    MessageContent Localize(LocalizationContext context);
}
