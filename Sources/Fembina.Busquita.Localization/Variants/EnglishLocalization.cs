using Fembina.Busquita.Localization.Factories;
using Falko.Talkie.Models.Messages.Contents;
using Falko.Talkie.Models.Messages.Contents.Styles;

namespace Fembina.Busquita.Localization.Variants;

public sealed class EnglishLocalization : ILocalization
{
    public ILocalizationContent About { get; } = LocalizationContent
        .Factory((context, builder) => builder
            .AddText(context.BowEmoji)
            .AddText(' ')
            .AddText("Hey! ", BoldTextStyle.FromTextRange)
            .AddText("I’m your music companion.")
            .AddTextLine(2)
            .AddText(context.HeartEmoji)
            .AddText(' ')
            .AddText("Just type the track name", BoldTextStyle.FromTextRange)
            .AddText(", and I’ll gladly find it for you!")
            .AddTextLine(2)
            .AddText(context.FlowerEmoji)
            .AddText(' ')
            .AddText("No hassle — just music and good vibes!"));
}
