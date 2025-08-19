using Fembina.Busquita.Localization.Factories;
using Falko.Talkie.Models.Messages.Contents;
using Falko.Talkie.Models.Messages.Contents.Styles;

namespace Fembina.Busquita.Localization.Variants;

public sealed class SpanishLocalization : ILocalization
{
    public ILocalizationContent About { get; } = LocalizationContent
        .Factory((context, builder) => builder
            .AddText(context.BowEmoji)
            .AddText(' ')
            .AddText("¡Hola! ", BoldTextStyle.FromTextRange)
            .AddText("Soy tu amiga musical.")
            .AddTextLine(2)
            .AddText(context.HeartEmoji)
            .AddText(' ')
            .AddText("Escribe el nombre de la canción ", BoldTextStyle.FromTextRange)
            .AddText("y con gusto la encontraré para ti.")
            .AddTextLine(2)
            .AddText(context.FlowerEmoji)
            .AddText(' ')
            .AddText("Sin complicaciones, solo música y buen rollo."));
}
