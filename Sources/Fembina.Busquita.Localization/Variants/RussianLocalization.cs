using Fembina.Busquita.Localization.Factories;
using Falko.Talkie.Models.Messages.Contents;
using Falko.Talkie.Models.Messages.Contents.Styles;

namespace Fembina.Busquita.Localization.Variants;

public sealed class RussianLocalization : ILocalization
{
    public ILocalizationContent About { get; } = LocalizationContent
        .Factory((context, builder) => builder
            .AddText(context.BowEmoji)
            .AddText(' ')
            .AddText("Приветик! ", BoldTextStyle.FromTextRange)
            .AddText("Я — твоя музыкальная подружка.")
            .AddTextLine(2)
            .AddText(context.HeartEmoji)
            .AddText(' ')
            .AddText("Напиши название трека ", BoldTextStyle.FromTextRange)
            .AddText("— и я с радостью найду его для тебя!")
            .AddTextLine(2)
            .AddText(context.FlowerEmoji)
            .AddText(' ')
            .AddText("Никаких заморочек — только музыка и хорошее настроение!"));
}
