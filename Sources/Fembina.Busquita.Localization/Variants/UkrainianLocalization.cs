using Fembina.Busquita.Localization.Factories;
using Talkie.Models.Messages.Contents;
using Talkie.Models.Messages.Contents.Styles;

namespace Fembina.Busquita.Localization.Variants;

public sealed class UkrainianLocalization : ILocalization
{
    public ILocalizationContent About { get; } = LocalizationContent
        .Factory((context, builder) => builder
            .AddText(context.BowEmoji)
            .AddText(' ')
            .AddText("Привіт! ", BoldTextStyle.FromTextRange)
            .AddText("Я — твоя музична подружка.")
            .AddTextLine(2)
            .AddText(context.HeartEmoji)
            .AddText(' ')
            .AddText("Напиши назву треку ", BoldTextStyle.FromTextRange)
            .AddText("— і я з радістю знайду його для тебе!")
            .AddTextLine(2)
            .AddText(context.FlowerEmoji)
            .AddText(' ')
            .AddText("Ніяких турбот — тільки музика та гарний настрій!"));
}
