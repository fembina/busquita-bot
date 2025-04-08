using System.Text;

namespace Fembina.Busquita.Storages.Caches;

public static class TrackNameBuilderCache
{
    public const int TrackMaxLength = 32;

    [ThreadStatic]
    private static StringBuilder? _builder;

    public static StringBuilder Reserve()
    {
        var builder = Interlocked.Exchange(ref _builder, null);

        if (builder is null) return new StringBuilder(TrackMaxLength);

        if (builder.Length > 0) builder.Clear();

        if (builder.Capacity > TrackMaxLength)  builder.EnsureCapacity(TrackMaxLength);

        return builder;
    }

    public static void Release(StringBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Interlocked.CompareExchange(ref _builder, builder, null);
    }
}
