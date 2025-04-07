using System.Collections.Frozen;

namespace Fembina.Busquita.Localization.Contexts;

public readonly struct LocalizationContext(FrozenDictionary<string, string> metadata)
{
    public string this[string key] => metadata.TryGetValue(key, out var data)
        ? data
        : throw new NotImplementedException($"Localization data not implemented in the context for the key '{key}'.");

    public string HeartEmoji => "\ud83d\udc96";

    public string BowEmoji => "\ud83c\udf80";

    public string FlowerEmoji => "\ud83c\udf38";
}
