using System.Globalization;
using Fembina.Busquita.Storages.Caches;

namespace Fembina.Busquita.Bot.Utils;

public static class TrackNameFormatter
{
    public const int TrackMaxLength = 32;

    public const int TrackMinLength = 3;

    private static readonly StringBuilderCache StringBuilderCache = new(TrackMaxLength);

    public static string FormatTextToTrackName(string text)
    {
        using var builderValue = StringBuilderCache.GetOrCreate();

        var builder = builderValue.Value;

        var textSpan = text.AsSpan();

        var previousIsSpace = false;

        foreach (var symbol in textSpan)
        {
            if (builder.Length >= TrackMaxLength) break;

            var symbolCategory = char.GetUnicodeCategory(symbol);

            if (IsTrackNameSymbol(symbolCategory))
            {
                builder.Append(symbol);
            }
            else if (symbolCategory is UnicodeCategory.SpaceSeparator)
            {
                if (previousIsSpace) continue;

                builder.Append(symbol);

                previousIsSpace = true;

                continue;
            }
            else if (symbolCategory is UnicodeCategory.ParagraphSeparator or UnicodeCategory.LineSeparator)
            {
                break;
            }
            else
            {
                if (previousIsSpace) continue;

                builder.Append(' ');

                previousIsSpace = true;

                continue;
            }

            previousIsSpace = false;
        }

        return builder.Length >= TrackMinLength
            ? builder.ToString()
            : string.Empty;
    }

    private static bool IsTrackNameSymbol(UnicodeCategory symbolCategory)
    {
        return symbolCategory
            is UnicodeCategory.UppercaseLetter
            or UnicodeCategory.LowercaseLetter
            or UnicodeCategory.TitlecaseLetter
            or UnicodeCategory.ModifierLetter
            or UnicodeCategory.OtherLetter
            or UnicodeCategory.LetterNumber
            or UnicodeCategory.DecimalDigitNumber
            or UnicodeCategory.OtherPunctuation
            or UnicodeCategory.DashPunctuation
            or UnicodeCategory.OpenPunctuation
            or UnicodeCategory.ClosePunctuation;
    }
}
