using System.Text;

namespace Fembina.Busquita.Storages.Caches;

public static class StringBuilderCache
{
    private const int Capacity = 256;

    [ThreadStatic]
    private static StringBuilder? _builder;

    public static StringBuilder Reserve()
    {
        var builder = Interlocked.Exchange(ref _builder, null);

        if (builder is null) return new StringBuilder(Capacity);

        if (builder.Length > 0) builder.Clear();

        if (builder.Capacity > Capacity)  builder.EnsureCapacity(Capacity);

        return builder;
    }

    public static void Release(StringBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Interlocked.CompareExchange(ref _builder, builder, null);
    }
}
