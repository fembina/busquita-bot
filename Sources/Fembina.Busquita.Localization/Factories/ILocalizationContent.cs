using Fembina.Busquita.Localization.Contexts;
using Talkie.Models.Messages.Contents;

namespace Fembina.Busquita.Localization.Factories;

public interface ILocalizationContent
{
    MessageContent Localize(LocalizationContext context);
}
