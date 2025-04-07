using System.Collections.Frozen;
using Fembina.Busquita.Localization.Factories;
using Talkie.Models.Messages.Contents;

namespace Fembina.Busquita.Localization.Contexts;

public sealed class LocalizedMessageContentBuilder(ILocalizationContent content)
{
    private readonly Dictionary<string, string> _metadata = [];

    public LocalizedMessageContentBuilder Set(string key, string data)
    {
        _metadata[key] = data;

        return this;
    }

    public MessageContent Build() => content
        .Localize(new LocalizationContext(_metadata
            .ToFrozenDictionary()));

    public static implicit operator MessageContent(LocalizedMessageContentBuilder builder) => builder.Build();

    public static implicit operator string(LocalizedMessageContentBuilder builder) => builder.Build();
}
