using System.Text;

namespace Fembina.Busquita.Storages.Caches;

public sealed class StringBuilderCache
{
    private readonly Action<StringBuilder> _trySetDelegate;

    private readonly int _capacity;

    private StringBuilder? _builder;

    public StringBuilderCache(int capacity = 256)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity, nameof(capacity));

        _capacity = capacity;
        _trySetDelegate = TrySet;
    }

    public ShotTimeValue<StringBuilder> GetOrCreate()
    {
        var builder = Interlocked.Exchange(ref _builder, null);

        if (builder is null) return new ShotTimeValue<StringBuilder>(new StringBuilder(_capacity), _trySetDelegate);

        if (builder.Length > 0) builder.Clear();

        if (builder.Capacity > _capacity) builder.EnsureCapacity(_capacity);

        return new ShotTimeValue<StringBuilder>(builder, _trySetDelegate);
    }

    private void TrySet(StringBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        Interlocked.CompareExchange(ref _builder, builder, null);
    }
}
